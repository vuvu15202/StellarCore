using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stellar.Shared.Repositories;
using UserService.Domain.Models.Entities;
using UserService.Domain.Services.Persistence;
using UserService.Infrastructure.Database;

namespace UserService.Infrastructure.Services.Repositories
{
    public class RelationPlanFunctionRepository : CrudRepository<RelationPlanFunction, Guid>, RelationPlanFunctionPersistence
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
