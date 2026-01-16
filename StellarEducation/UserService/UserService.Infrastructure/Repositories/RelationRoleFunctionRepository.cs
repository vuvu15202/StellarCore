using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Persistence;
using Stellar.Shared.Repositories;

namespace UserService.Infrastructure.Repositories
{
    public class RelationRoleFunctionRepository : CrudRepository<RelationRoleFunction, Guid>, IRelationRoleFunctionRepository
    {
        private readonly UserDbContext _context;

        public RelationRoleFunctionRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task DeleteByRoleId(Guid roleId)
        {
            var entities = await _context.RoleFunctions.Where(x => x.RoleId == roleId).ToListAsync();
            _context.RoleFunctions.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RelationRoleFunction>> FindByRoleId(Guid roleId)
        {
            return await _context.RoleFunctions.Where(x => x.RoleId == roleId).ToListAsync();
        }
    }
}
