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
    public class RelationPlanFunctionRepository : CrudRepository<RelationPlanFunction, Guid>, IRelationPlanFunctionRepository
    {
        private readonly UserDbContext _context;

        public RelationPlanFunctionRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<RelationPlanFunction>> FindByPlanId(Guid planId)
        {
            return await _context.PlanFunctions.Where(x => x.PlanId == planId).ToListAsync();
        }
    }
}
