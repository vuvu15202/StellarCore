using CategoryService.Application.DTOs;

namespace CategoryService.Application.Interfaces
{
    public interface ICategoryTypeService
    {
        Task<List<CategoryTypeDTO>> GetAllAsync();
        Task<CategoryTypeDTO?> GetByIdAsync(Guid id);
        Task<CategoryTypeDTO> CreateAsync(CategoryTypeDTO dto);
        Task UpdateAsync(CategoryTypeDTO dto);
        Task DeleteAsync(Guid id);
    }

    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllAsync(string categoryTypeMapping);
        Task<List<CategoryDTO>> GetByParentIdAsync(Guid? parentId);
        Task<CategoryDTO?> GetByIdAsync(Guid id);
        Task<CategoryDTO> CreateAsync(string categoryTypeMapping, CategoryDTO dto);
        Task UpdateAsync(string categoryTypeMapping, CategoryDTO dto);
        Task DeleteAsync(string categoryTypeMapping, Guid id);
    }
}
