using System;

namespace UserService.Application.DTOs.Requests
{
    public class SubscriptionRequest
    {
        public Guid UserId { get; set; }
        public Guid PlanId { get; set; }
    }
}
