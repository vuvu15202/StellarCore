using System;
using Stellar.Shared.Repositories;
using UserService.Domain.Models.Entities;
using UserService.Domain.Services.Persistence;
using UserService.Infrastructure.Database;

namespace UserService.Infrastructure.Services.Repositories
{
    public class PlanRepository : CrudRepository<Plan, Guid>, PlanPersistence
    {
        public PlanRepository(UserDbContext context) : base(context)
        {
        }
    }
}
