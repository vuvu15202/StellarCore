using UserService.Domain.Entities;
using Stellar.Shared.Interfaces.Persistence;
using System;
using System.Threading.Tasks;

namespace UserService.Domain.Repositories
{
    public interface IUserPlanSubscriptionRepository : ICrudPersistence<UserPlanSubscription, Guid>, IGetAllPersistence<UserPlanSubscription>
    {
        Task<UserPlanSubscription?> GetActiveSubscriptionByUserId(Guid userId);
    }
}
