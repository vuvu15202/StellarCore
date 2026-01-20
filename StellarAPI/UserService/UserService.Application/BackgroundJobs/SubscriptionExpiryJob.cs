using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Services.Persistence;

namespace UserService.Application.BackgroundJobs
{
    public class SubscriptionExpiryJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SubscriptionExpiryJob> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1);//1 tiếng 1 lần

        public SubscriptionExpiryJob(IServiceProvider serviceProvider, ILogger<SubscriptionExpiryJob> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Subscription Expiry Job is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DeactivateExpiredSubscriptions();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while deactivating expired subscriptions.");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }

            _logger.LogInformation("Subscription Expiry Job is stopping.");
        }

        private async Task DeactivateExpiredSubscriptions()
        {
            _logger.LogInformation("Checking for expired subscriptions...");

            using (var scope = _serviceProvider.CreateScope())
            {
                var subscriptionRepository = scope.ServiceProvider.GetRequiredService<UserPlanSubscriptionPersistence>();
                
                var expiredSubscriptions = await subscriptionRepository.Query()
                    .Where(s => s.IsActive && s.EndDate < DateTime.UtcNow)
                    .ToListAsync();

                if (expiredSubscriptions.Any())
                {
                    _logger.LogInformation("Found {Count} expired subscriptions. Deactivating...", expiredSubscriptions.Count);
                    
                    foreach (var sub in expiredSubscriptions)
                    {
                        sub.IsActive = false;
                        subscriptionRepository.Save(sub);
                    }
                    
                    _logger.LogInformation("Expired subscriptions deactivated successfully.");
                }
                else
                {
                    _logger.LogInformation("No expired subscriptions found.");
                }
            }
        }
    }
}
