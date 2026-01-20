using System;
using UserService.Domain.Entities;
using Stellar.Shared.Interfaces;
using Stellar.Shared.Interfaces.Persistence;

namespace UserService.Domain.Repositories
{
    public interface IFunctionGroupRepository : ICrudPersistence<FunctionGroup, Guid>, IGetAllPersistence<FunctionGroup>
    {
    }
}
