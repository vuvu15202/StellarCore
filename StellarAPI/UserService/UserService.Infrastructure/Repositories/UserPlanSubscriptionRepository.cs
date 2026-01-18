using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Persistence;
using Stellar.Shared.Repositories;

namespace UserService.Infrastructure.Repositories
{
    public class UserPlanSubscriptionRepository : CrudRepository<UserPlanSubscription, Guid>, IUserPlanSubscriptionRepository
    {
        private readonly UserDbContext _context;

        public UserPlanSubscriptionRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<UserPlanSubscription?> GetActiveSubscriptionByUserId(Guid userId)
        {
            return await _context.UserSubscriptions
                .Where(x => x.UserId == userId && x.IsActive && x.EndDate > DateTime.UtcNow)
                .OrderByDescending(x => x.EndDate)
                .FirstOrDefaultAsync();
        }
    }
}
