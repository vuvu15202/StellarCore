using Stellar.Shared.Interfaces.Persistence;
using System;
using System.Threading.Tasks;
using UserService.Domain.Models.Entities;

namespace UserService.Domain.Services.Persistence
{
    public interface UserPlanSubscriptionPersistence : ICrudPersistence<UserPlanSubscription, Guid>, IGetAllPersistence<UserPlanSubscription>
    {
        Task<UserPlanSubscription?> GetActiveSubscriptionByUserId(Guid userId);
    }
}
