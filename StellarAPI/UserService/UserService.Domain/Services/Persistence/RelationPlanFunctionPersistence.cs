using Stellar.Shared.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Domain.Models.Entities;

namespace UserService.Domain.Services.Persistence
{
    public interface RelationPlanFunctionPersistence : ICrudPersistence<RelationPlanFunction, Guid>, IGetAllPersistence<RelationPlanFunction>
    {
        Task<List<RelationPlanFunction>> FindByPlanId(Guid planId);
    }
}
