using Stellar.Shared.Interfaces.Persistence;
using UserService.Domain.Entities;

namespace UserService.Domain.Repositories
{
    public interface IRoleRepository : ICrudPersistence<Role, Guid>, IGetAllPersistence<Role>
    {
    }
}
