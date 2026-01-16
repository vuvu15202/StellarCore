using AutoMapper;
using CategoryService.Application.DTOs;
using CategoryService.Application.Interfaces;
using CategoryService.Domain.Entities;
using CategoryService.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CategoryService.Application.Services
{
    public class CategoryTypeService : ICategoryTypeService
    {
        private readonly ICategoryTypeRepository _repository;
        private readonly IMapper _mapper;

        public CategoryTypeService(ICategoryTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CategoryTypeDTO>> GetAllAsync()
        {
            var entities = await Task.FromResult(_repository.Query().ToList());
            return _mapper.Map<List<CategoryTypeDTO>>(entities);
        }

        public async Task<CategoryTypeDTO?> GetByIdAsync(Guid id)
        {
            var entity = await Task.FromResult(_repository.FindById(id));
            return _mapper.Map<CategoryTypeDTO>(entity);
        }

        public async Task<CategoryTypeDTO> CreateAsync(CategoryTypeDTO dto)
        {
            var entity = _mapper.Map<CategoryType>(dto);
            var saved = await Task.FromResult(_repository.Save(entity));
            return _mapper.Map<CategoryTypeDTO>(saved);
        }

        public async Task UpdateAsync(CategoryTypeDTO dto)
        {
            var entity = _mapper.Map<CategoryType>(dto);
            await Task.Run(() => _repository.Save(entity));
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await Task.FromResult(_repository.FindById(id));
            if (entity != null)
            {
                await Task.Run(() => _repository.Delete(entity));
            }
        }
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepositoryProvider _repositoryProvider;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepositoryProvider repositoryProvider, IMapper mapper)
        {
            _repositoryProvider = repositoryProvider;
            _mapper = mapper;
        }

        public async Task<List<CategoryDTO>> GetAllAsync(string mapping)
        {
            var repo = _repositoryProvider.GetRepository(mapping);
            var entities = await Task.FromResult(((dynamic)repo).Query().ToList());
            var dtos = _mapper.Map<List<CategoryDTO>>(entities);

            return BuildTree(dtos);
        }

        public async Task<List<CategoryDTO>> GetByParentIdAsync(Guid? parentId)
        {
            // Note: This requires mapping context in a real scenario, 
            // but for now we follow the pattern of the GetAll tree.
            throw new NotImplementedException("Use GetAllAsync with mapping to get hierarchical tree.");
        }

        public async Task<CategoryDTO?> GetByIdAsync(Guid id)
        {
             throw new NotImplementedException("GetById requires mapping context or global lookup.");
        }

        public async Task<CategoryDTO> CreateAsync(string mapping, CategoryDTO dto)
        {
            var repo = _repositoryProvider.GetRepository(mapping);
            var entityType = _repositoryProvider.GetEntityType(mapping);
            
            var entity = _mapper.Map(dto, typeof(CategoryDTO), entityType);
            var saved = await Task.FromResult(((dynamic)repo).Save(entity));
            return _mapper.Map<CategoryDTO>(saved);
        }

        public async Task UpdateAsync(string mapping, CategoryDTO dto)
        {
            var repo = _repositoryProvider.GetRepository(mapping);
            var entityType = _repositoryProvider.GetEntityType(mapping);

            var entity = _mapper.Map(dto, typeof(CategoryDTO), entityType);
            await Task.Run(() => ((dynamic)repo).Save(entity));
        }

        public async Task DeleteAsync(string mapping, Guid id)
        {
            var repo = _repositoryProvider.GetRepository(mapping);
            var entity = await Task.FromResult(((dynamic)repo).FindById(id));
            if (entity != null)
            {
                await Task.Run(() => ((dynamic)repo).Delete(entity));
            }
        }

        private List<CategoryDTO> BuildTree(List<CategoryDTO> allNodes)
        {
            var nodeMap = allNodes.ToDictionary(n => n.Id);
            var rootNodes = new List<CategoryDTO>();

            foreach (var node in allNodes.OrderBy(n => n.Sort ?? int.MaxValue))
            {
                if (node.ParentId.HasValue && nodeMap.TryGetValue(node.ParentId.Value, out var parent))
                {
                    parent.Children.Add(node);
                    node.Parent = new CategoryDTO 
                    { 
                        Id = parent.Id, 
                        Name = parent.Name, 
                        Code = parent.Code,
                        Path = parent.Path
                    };
                }
                else
                {
                    rootNodes.Add(node);
                }
            }

            return rootNodes;
        }
    }
}
