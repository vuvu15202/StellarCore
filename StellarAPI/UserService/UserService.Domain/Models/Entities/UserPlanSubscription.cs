using System;
using System.ComponentModel.DataAnnotations.Schema;
using Stellar.Shared.Models;

namespace UserService.Domain.Models.Entities
{
    [Table("user_plan_subscriptions")]
    public class UserPlanSubscription : AuditingEntity
    {
        public Guid UserId { get; set; }
        public Guid PlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
