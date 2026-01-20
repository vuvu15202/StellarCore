using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Domain.Enums;
using Stellar.Shared.Services;
using Stellar.Shared.Interfaces.Persistence;
using Stellar.Shared.Models;
using Stellar.Shared.Interfaces;

namespace UserService.Application.Services
{
    public class RoleService : BaseService<Role, Guid, RoleResponse, RoleRequest, RoleResponse>, IRoleService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFunctionRepository _functionRepository;
        private readonly IFunctionService _functionService;
        private readonly IUserPlanSubscriptionRepository _subscriptionRepository;
        private readonly IRelationPlanFunctionRepository _planFunctionRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IFunctionGroupRepository _functionGroupRepository;
        private readonly IRoleRepository _rolePersistence;

        public RoleService(
            IUserRepository userRepository,
            IFunctionRepository functionRepository,
            IFunctionService functionService,
            IUserPlanSubscriptionRepository subscriptionRepository,
            IRelationPlanFunctionRepository planFunctionRepository,
            IPlanRepository planRepository,
            IFunctionGroupRepository functionGroupRepository,
            IRoleRepository rolePersistence)
        {
            _userRepository = userRepository;
            _functionRepository = functionRepository;
            _functionService = functionService;
            _subscriptionRepository = subscriptionRepository;
            _planFunctionRepository = planFunctionRepository;
            _planRepository = planRepository;
            _functionGroupRepository = functionGroupRepository;
            _rolePersistence = rolePersistence;
        }

        public override ICrudPersistence<Role, Guid> GetCrudPersistence() => _rolePersistence;

        public override void MappingCreate(HeaderContext context, Role entity, RoleRequest request)
        {
            entity.Id = Guid.NewGuid();
            entity.Name = request.Name;
            entity.Description = request.Description;
        }

        public override void MappingUpdateEntity(HeaderContext context, Role entity, RoleRequest request)
        {
            entity.Name = request.Name;
            entity.Description = request.Description;
        }

        public override RoleResponse MappingResponse(HeaderContext context, Role entity)
        {
            return new RoleResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }



        public async Task AssignFunctionGroup(Guid userId, Guid functionGroupId)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {userId} does not exist.");
            }

            if (!_functionGroupRepository.ExistsById(functionGroupId))
            {
                throw new ArgumentException($"FunctionGroup with ID {functionGroupId} does not exist.");
            }

            user.FunctionGroupId = functionGroupId;
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task<MenuResponse> GetPermission(Guid userId)
        {
            var response = new MenuResponse();
            var functionPermissionIds = new HashSet<Guid>();
            var ignoreFunctionIds = new HashSet<Guid>();

            // 1. Get User and primary FunctionGroup
            var user = await _userRepository.FindByIdAsync(userId);
            if (user == null || !user.FunctionGroupId.HasValue)
            {
                return response;
            }

            var primaryGroup = _functionGroupRepository.FindById(user.FunctionGroupId.Value);
            if (primaryGroup == null)
            {
                return response;
            }

            response.FunctionGroupId = primaryGroup.Id;
            response.FunctionGroupName = primaryGroup.Name;

            // 2. Collect functions from primary group
            if (primaryGroup.DefaultFunctionIds != null)
            {
                foreach (var fid in primaryGroup.DefaultFunctionIds)
                {
                    functionPermissionIds.Add(fid);
                }
            }

            // 3. Collect ignore rules from primary group
            if (primaryGroup.RuleIgnoreFunctionIds != null)
            {
                foreach (var fid in primaryGroup.RuleIgnoreFunctionIds)
                {
                    ignoreFunctionIds.Add(fid);
                }
            }

            // 4. Collect from parent group (DefaultFunctionGroupId) if exists
            if (primaryGroup.DefaultFunctionGroupId.HasValue)
            {
                var parentGroup = _functionGroupRepository.FindById(primaryGroup.DefaultFunctionGroupId.Value);
                if (parentGroup != null && parentGroup.DefaultFunctionIds != null)
                {
                    foreach (var fid in parentGroup.DefaultFunctionIds)
                    {
                        functionPermissionIds.Add(fid);
                    }
                }
            }

            // 5. Collect from Plan
            Guid? activePlanId = null;
            var activeSub = await _subscriptionRepository.GetActiveSubscriptionByUserId(userId);
            if (activeSub != null)
            {
                activePlanId = activeSub.PlanId;
            }
            else
            {
                // Fallback to Trial Plan matching PlanType based on legacy roles or defaults
                // For now, keep it simple as before: find first trial plan
                var trialPlans = await _planRepository.Query().ToListAsync();
                var defaultTrial = trialPlans.FirstOrDefault(p => p.Price == 0 && p.IsActive);
                if (defaultTrial != null)
                {
                    activePlanId = defaultTrial.Id;
                }
            }

            if (activePlanId.HasValue)
            {
                var planFunctionIds = await _planFunctionRepository.FindByPlanId(activePlanId.Value);
                foreach (var pf in planFunctionIds)
                {
                    functionPermissionIds.Add(pf.FunctionId);
                }
            }

            // 6. Subtract Ignore list
            functionPermissionIds.ExceptWith(ignoreFunctionIds);

            if (!functionPermissionIds.Any())
            {
                return response;
            }

            // Get function entities
            var functions = await _functionRepository.FindAllByIdIn(functionPermissionIds.ToList());

            // User Java's buildMenuFromChucNangs logic mapping
            // Filter Permissions (Action)
            response.Permissions = functions
                .Where(f => f.Type == FunctionType.Action)
                .Select(f => f.Code)
                .Distinct()
                .ToList();

            // Filter Menus (Navigation)
            var navigationFunctions = functions
                .Where(f => f.Type == FunctionType.Navigation)
                .OrderBy(f => f.SortOrder) // Assuming SortOrder exists based on Function entity
                .ToList();
            
            response.Menus = BuildTreeFromFunctions(navigationFunctions);

            return response;
        }

        private List<FunctionResponse> BuildTreeFromFunctions(List<Function> functions)
        {
            var nodeMap = new Dictionary<Guid, FunctionResponse>();
            // Map to Response DTOs
            foreach (var func in functions)
            {
                // We need to map Function to FunctionResponse here. 
                // Since _functionService.MappingResponse might require dependencies we can't easily access or mock, 
                // and we need a clean tree, we'll map manually or reuse if possible.
                // Reusing _functionService.MappingResponse(null, func) as per existing code hint, 
                // but passing null context might be risky if it uses it.
                // Let's implement a simple mapping here to sure.

                 var dto = new FunctionResponse
                 {
                     Id = func.Id,
                     Code = func.Code,
                     Name = func.Name,
                     Path = func.Path,
                     SortOrder = func.SortOrder,
                     SortPath = func.SortPath,
                     Icon = func.Icon,
                     Description = func.Description,
                     HierarchyPath = func.HierarchyPath,
                     Category = func.Category,
                     ParentId = func.ParentId,
                     Type = func.Type,
                     CreatedBy = func.CreatedBy,
                     LastModifiedBy = func.UpdatedBy,
                     CreatedAt = func.CreatedAt,
                     LastModifiedAt = func.UpdatedAt,
                     Children = new List<FunctionResponse>()
                 };
                 nodeMap[func.Id] = dto;
            }

            var roots = new List<FunctionResponse>();

            foreach (var node in nodeMap.Values)
            {
                 if (node.ParentId.HasValue && nodeMap.ContainsKey(node.ParentId.Value))
                 {
                     nodeMap[node.ParentId.Value].Children.Add(node);
                 }
                 else
                 {
                     roots.Add(node);
                 }
            }

            return roots;
        }
    }
}
