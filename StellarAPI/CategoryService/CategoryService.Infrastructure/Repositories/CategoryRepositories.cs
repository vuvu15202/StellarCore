using CategoryService.Domain.Entities;
using CategoryService.Domain.Repositories;
using CategoryService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Stellar.Shared.Repositories;

namespace CategoryService.Infrastructure.Repositories
{
    public class CategoryTypeRepository : CrudRepository<CategoryType, Guid>, ICategoryTypeRepository
    {
        public CategoryTypeRepository(CategoryDbContext context) : base(context) { }
    }

    public class BaseCategoryRepository<T> : CrudRepository<T, Guid>, IBaseCategoryRepository<T> where T : BaseCategory
    {
        public BaseCategoryRepository(CategoryDbContext context) : base(context) { }

        public async Task<List<T>> GetByParentIdAsync(Guid? parentId)
        {
            return await DbSet.Where(x => x.ParentId == parentId).ToListAsync();
        }

        public async Task<List<T>> GetByCategoryTypeIdAsync(Guid categoryTypeId)
        {
            return await DbSet.Where(x => x.CategoryTypeId == categoryTypeId).ToListAsync();
        }
    }
}
