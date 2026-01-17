using UserService.Domain.Entities;
using Stellar.Shared.Interfaces.Persistence;

namespace UserService.Domain.Repositories
{
    public interface IPlanRepository : ICrudPersistence<Plan, Guid>, IGetAllPersistence<Plan>
    {
    }
}
