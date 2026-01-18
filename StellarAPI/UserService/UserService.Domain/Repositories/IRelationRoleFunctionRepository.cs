using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stellar.Shared.Interfaces;
using Stellar.Shared.Interfaces.Persistence;
using UserService.Domain.Entities;

namespace UserService.Domain.Repositories
{
    public interface IRelationRoleFunctionRepository : ICrudPersistence<RelationRoleFunction, Guid>, IGetAllPersistence<RelationRoleFunction>
    {
        Task DeleteByRoleId(Guid roleId);
        Task<List<RelationRoleFunction>> FindByRoleId(Guid roleId);
    }
}
