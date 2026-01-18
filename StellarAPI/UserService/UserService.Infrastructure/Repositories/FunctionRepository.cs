using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Persistence;
using Stellar.Shared.Repositories;

namespace UserService.Infrastructure.Repositories
{
    public class FunctionRepository : CrudRepository<Function, Guid>, IFunctionRepository
    {
        public FunctionRepository(UserDbContext context) : base(context)
        {
        }

        public async Task<List<Function>> GetChildrenByParentIds(List<Guid> parentIds)
        {
            return await DbSet.Where(x => x.ParentId != null && parentIds.Contains(x.ParentId.Value))
                .OrderBy(x => x.SortOrder)
                .ToListAsync();
        }

        public async Task<List<Function>> GetFunctionsByUserId(Guid userId)
        {
            return await Task.FromResult(new List<Function>());
        }

        public async Task<List<Function>> FindByParentId(Guid parentId)
        {
            return await DbSet.Where(x => x.ParentId == parentId)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();
        }

        public async Task<List<Function>> FindAllByIdIn(List<Guid> ids)
        {
            return await DbSet.Where(x => ids.Contains(x.Id)).ToListAsync();
        }
    }
}
