using System;
using System.Threading.Tasks;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Application.Usecases.Interfaces;
using UserService.Domain.Services.Persistence;
using UserService.Domain.Models.Entities;

namespace UserService.Application.Usecases
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly UserPlanSubscriptionPersistence _subscriptionRepository;
        private readonly PlanPersistence _planRepository;

        public SubscriptionService(
            UserPlanSubscriptionPersistence subscriptionRepository,
            PlanPersistence planRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _planRepository = planRepository;
        }

        public async Task<SubscriptionResponse> Subscribe(SubscriptionRequest request)
        {
            var plan = _planRepository.FindById(request.PlanId);
            if (plan == null) throw new ArgumentException("Plan not found");

            // Deactivate existing active subscriptions
            var activeSub = await _subscriptionRepository.GetActiveSubscriptionByUserId(request.UserId);
            if (activeSub != null)
            {
                activeSub.IsActive = false;
                _subscriptionRepository.Save(activeSub);
            }

            var sub = new UserPlanSubscription
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                PlanId = request.PlanId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(plan.DurationDays),
                IsActive = true
            };

            _subscriptionRepository.Save(sub);

            return new SubscriptionResponse
            {
                Id = sub.Id,
                UserId = sub.UserId,
                PlanId = sub.PlanId,
                PlanName = plan.Name,
                StartDate = sub.StartDate,
                EndDate = sub.EndDate,
                IsActive = sub.IsActive
            };
        }

        public async Task<SubscriptionResponse?> GetActiveSubscription(Guid userId)
        {
            var sub = await _subscriptionRepository.GetActiveSubscriptionByUserId(userId);
            if (sub == null) return null;

            var plan = _planRepository.FindById(sub.PlanId);

            return new SubscriptionResponse
            {
                Id = sub.Id,
                UserId = sub.UserId,
                PlanId = sub.PlanId,
                PlanName = plan?.Name ?? "Unknown",
                StartDate = sub.StartDate,
                EndDate = sub.EndDate,
                IsActive = sub.IsActive
            };
        }
    }
}
