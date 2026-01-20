using Stellar.Shared.Interfaces.Persistence;
using UserService.Domain.Models.Entities;

namespace UserService.Domain.Services.Persistence
{
    public interface RolePersistence : ICrudPersistence<Role, Guid>, IGetAllPersistence<Role>
    {
    }
}
