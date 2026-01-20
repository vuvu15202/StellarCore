using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using Stellar.Shared.Services;
using Stellar.Shared.Models;
using Stellar.Shared.Interfaces;
using Stellar.Shared.Interfaces.Persistence;
using Stellar.Shared.Utils;
using UserService.Domain.Models.Entities;
using UserService.Domain.Services.Persistence;
using UserService.Application.Usecases.Interfaces;

namespace UserService.Application.Usecases
{
    public class FunctionService : IFunctionService
    {
        private readonly FunctionPersistence _persistence;
        
        public FunctionService(FunctionPersistence persistence)
        {
            _persistence = persistence;
        }

        public ICrudPersistence<Function, Guid> GetCrudPersistence() => _persistence;
        public IGetAllPersistence<Function> GetGetAllPersistence() => _persistence;

        public async Task<List<Function>> GetFunctionIds(Guid userId)
        {
            return await _persistence.GetFunctionsByUserId(userId);
        }

        public void ValidateCreate(HeaderContext context, Function entity, FunctionRequest request)
        {
            bool codeExists = _persistence.Query().Any(x => x.Code == request.Code);
            Validators.FunctionsValidator.ValidateCreate(context, entity, request, codeExists);
        }

        public void ValidateUpdateRequest(HeaderContext context, Guid id, Function entity, FunctionRequest request)
        {
            bool codeExists = _persistence.Query().Any(x => x.Code == request.Code && x.Id != id);
            Validators.FunctionsValidator.ValidateUpdate(context, id, entity, request, codeExists);
        }

        public void MappingCreate(HeaderContext context, Function entity, FunctionRequest request)
        {
            FnCommon.CopyProperties(entity, request);
            entity.Id = Guid.NewGuid();
        }

        public void PostCreateHandler(HeaderContext context, Function entity, Guid id, FunctionRequest request)
        {
            UpdateHierarchyPath(entity);
            _persistence.Save(entity);
        }

        public void MappingUpdateEntity(HeaderContext context, Function entity, FunctionRequest request)
        {
            Guid? oldParentId = entity.ParentId;
            FnCommon.CopyProperties(entity, request);
            
            if (oldParentId != request.ParentId)
            {
                UpdateHierarchyPath(entity);
            }
        }

        public void PostUpdateHandler(HeaderContext context, Function entity, Guid id, FunctionRequest request)
        {
            if (entity.HierarchyPath != null) {
                UpdateChildrenPath(entity.Id, entity.HierarchyPath).Wait();
            }
        }

        private void UpdateHierarchyPath(Function entity)
        {
            string newHierarchyPath = entity.Id.ToString();
            if (entity.ParentId.HasValue)
            {
                var parent = _persistence.FindById(entity.ParentId.Value);
                if (parent != null)
                {
                    newHierarchyPath = $"{parent.HierarchyPath}/{entity.Id}";
                }
            }
            entity.HierarchyPath = newHierarchyPath;
        }

        private async Task UpdateChildrenPath(Guid parentId, string parentHierarchyPath)
        {
            var children = await _persistence.FindByParentId(parentId);
            foreach (var child in children)
            {
                string newHierarchyPath = $"{parentHierarchyPath}/{child.Id}";
                child.HierarchyPath = newHierarchyPath;
                _persistence.Save(child);
                await UpdateChildrenPath(child.Id, newHierarchyPath);
            }
        }

        public FunctionResponse MappingResponse(HeaderContext context, Function entity)
        {
            return new FunctionResponse
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                Path = entity.Path,
                SortOrder = entity.SortOrder,
                SortPath = entity.SortPath,
                Icon = entity.Icon,
                Description = entity.Description,
                HierarchyPath = entity.HierarchyPath,
                Category = entity.Category,
                ParentId = entity.ParentId,
                Type = entity.Type,
                CreatedBy = entity.CreatedBy,
                CreatedAt = entity.CreatedAt,
                LastModifiedBy = entity.UpdatedBy,
                LastModifiedAt = entity.UpdatedAt
            };
        }

        public Page<FunctionResponse> GetAll(HeaderContext context, string? search, int page, int size, string? sort, Dictionary<string, object> filter)
        {
            var query = _persistence.Query();
            
            bool hasSearch = !string.IsNullOrWhiteSpace(search);
            
            if (hasSearch)
            {
                query = query.Where(x => x.Name.Contains(search!) || x.Code.Contains(search!));
            }
            else
            {
                query = query.Where(x => x.ParentId == null);
            }

            var items = query.OrderBy(x => x.SortOrder).ToList();
            var responses = items.Select(x => MappingResponse(context, x)).ToList();
            var responseMap = responses.ToDictionary(x => x.Id);

            if (hasSearch)
            {
                ProcessSearchWithAncestors(responses, responseMap).Wait();
            }
            else
            {
                ProcessWithoutSearch(items, responseMap).Wait();
            }

            var tree = BuildTree(responseMap);
            
            return new Page<FunctionResponse>(tree, 1, tree.Count, tree.Count);
        }

        private async Task ProcessWithoutSearch(List<Function> items, Dictionary<Guid, FunctionResponse> responseMap)
        {
            var ids = items.Select(x => x.Id).ToList();
            var children = await _persistence.GetChildrenByParentIds(ids);
            foreach (var child in children)
            {
                if (!responseMap.ContainsKey(child.Id))
                {
                    var res = MappingResponse(null!, child);
                    responseMap[child.Id] = res;
                }
            }
        }

        private async Task ProcessSearchWithAncestors(List<FunctionResponse> content, Dictionary<Guid, FunctionResponse> responseMap)
        {
            var allAncestorIds = new HashSet<Guid>();
            foreach (var item in content)
            {
                if (!string.IsNullOrEmpty(item.HierarchyPath))
                {
                    var parts = item.HierarchyPath.Split('/');
                    foreach (var part in parts)
                    {
                        if (Guid.TryParse(part, out Guid ancestorId) && !responseMap.ContainsKey(ancestorId))
                        {
                            allAncestorIds.Add(ancestorId);
                        }
                    }
                }
            }

            if (allAncestorIds.Any())
            {
                var ancestors = await _persistence.FindAllByIdIn(allAncestorIds.ToList());
                foreach (var ancestor in ancestors)
                {
                    if (!responseMap.ContainsKey(ancestor.Id))
                    {
                        responseMap[ancestor.Id] = MappingResponse(null!, ancestor);
                    }
                }
            }
        }

        private List<FunctionResponse> BuildTree(Dictionary<Guid, FunctionResponse> responseMap)
        {
            var rootNodes = new List<FunctionResponse>();
            var allNodes = responseMap.Values.OrderBy(x => x.SortOrder).ToList();

            foreach (var node in allNodes)
            {
                if (node.ParentId.HasValue && responseMap.TryGetValue(node.ParentId.Value, out var parent))
                {
                    parent.Children.Add(node);
                }
                else
                {
                    rootNodes.Add(node);
                }
            }

            return rootNodes;
        }

        public RES GetById<RES>(HeaderContext context, Guid id) 
        {
            var entity = _persistence.FindById(id);
            if (entity == null) return default!;
            return (RES)(object)MappingResponse(context, entity);
        }
        public void Delete(HeaderContext context, Guid id)
        {
             var entity = _persistence.FindById(id);
             if (entity != null) 
             {
                 _persistence.Delete(entity);
             }
        }
    }
}
