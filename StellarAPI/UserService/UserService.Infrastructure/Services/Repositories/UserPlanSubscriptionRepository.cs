using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stellar.Shared.Repositories;
using UserService.Domain.Models.Entities;
using UserService.Domain.Services.Persistence;
using UserService.Infrastructure.Database;

namespace UserService.Infrastructure.Services.Repositories
{
    public class UserPlanSubscriptionRepository : CrudRepository<UserPlanSubscription, Guid>, UserPlanSubscriptionPersistence
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
