using System;
using Stellar.Shared.Repositories;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories
{
    public class FunctionGroupRepository : CrudRepository<FunctionGroup, Guid>, IFunctionGroupRepository
    {
        public FunctionGroupRepository(UserDbContext dbContext) : base(dbContext)
        {
        }
    }
}
