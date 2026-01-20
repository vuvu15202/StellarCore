using System;
using Stellar.Shared.Repositories;
using UserService.Domain.Models.Entities;
using UserService.Domain.Services.Persistence;
using UserService.Infrastructure.Database;

namespace UserService.Infrastructure.Services.Repositories
{
    public class FunctionGroupRepository : CrudRepository<FunctionGroup, Guid>, FunctionGroupPersistence
    {
        public FunctionGroupRepository(UserDbContext dbContext) : base(dbContext)
        {
        }
    }
}
