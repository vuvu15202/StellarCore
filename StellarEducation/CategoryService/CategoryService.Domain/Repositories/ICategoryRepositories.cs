using CategoryService.Domain.Entities;
using Stellar.Shared.Interfaces;
using Stellar.Shared.Interfaces.Persistence;

namespace CategoryService.Domain.Repositories
{
    public interface ICategoryTypeRepository : ICrudPersistence<CategoryType, Guid>, IGetAllPersistence<CategoryType>
    {
    }

    public interface IBaseCategoryRepository<T> : ICrudPersistence<T, Guid>, IGetAllPersistence<T> where T : BaseCategory
    {
        Task<List<T>> GetByParentIdAsync(Guid? parentId);
        Task<List<T>> GetByCategoryTypeIdAsync(Guid categoryTypeId);
    }
}
