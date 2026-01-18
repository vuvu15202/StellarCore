using UserService.Domain.Entities;
using Stellar.Shared.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserService.Domain.Repositories
{
    public interface IRelationPlanFunctionRepository : ICrudPersistence<RelationPlanFunction, Guid>, IGetAllPersistence<RelationPlanFunction>
    {
        Task<List<RelationPlanFunction>> FindByPlanId(Guid planId);
    }
}
