using System;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Persistence;
using Stellar.Shared.Repositories;

namespace UserService.Infrastructure.Repositories
{
    public class PlanRepository : CrudRepository<Plan, Guid>, IPlanRepository
    {
        public PlanRepository(UserDbContext context) : base(context)
        {
        }
    }
}
