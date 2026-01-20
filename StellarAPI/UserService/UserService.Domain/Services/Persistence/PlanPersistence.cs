using Stellar.Shared.Interfaces.Persistence;
using UserService.Domain.Models.Entities;

namespace UserService.Domain.Services.Persistence
{
    public interface PlanPersistence : ICrudPersistence<Plan, Guid>, IGetAllPersistence<Plan>
    {
    }
}
