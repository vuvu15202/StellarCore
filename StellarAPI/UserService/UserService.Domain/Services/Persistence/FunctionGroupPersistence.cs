using System;
using Stellar.Shared.Interfaces;
using Stellar.Shared.Interfaces.Persistence;
using UserService.Domain.Models.Entities;

namespace UserService.Domain.Services.Persistence
{
    public interface FunctionGroupPersistence : ICrudPersistence<FunctionGroup, Guid>, IGetAllPersistence<FunctionGroup>
    {
    }
}
