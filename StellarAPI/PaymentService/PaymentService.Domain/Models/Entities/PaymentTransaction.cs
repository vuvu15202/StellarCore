using System.ComponentModel.DataAnnotations.Schema;
using PaymentService.Domain.Enums;
using Stellar.Shared.Models;

namespace PaymentService.Domain.Models.Entities
{
    [Table("payment_transactions")]
    public class PaymentTransaction : AuditingEntity
    {
        public string OrderId { get; set; } = null!;
        public string? TransactionId { get; set; } // From Gateway
        public decimal Amount { get; set; }
        public TransactionStatus Status { get; set; }
        public PaymentMethod Method { get; set; }
        public Guid UserId { get; set; }
        public Guid PlanId { get; set; }
        [Column(TypeName = "ntext")]
        public string? Description { get; set; }
        public string? PaymentUrl { get; set; } // To redirect user
    }
}
