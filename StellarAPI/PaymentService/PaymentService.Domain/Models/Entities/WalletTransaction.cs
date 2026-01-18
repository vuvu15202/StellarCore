using System.ComponentModel.DataAnnotations.Schema;
using PaymentService.Domain.Enums;
using Stellar.Shared.Models;

namespace PaymentService.Domain.Models.Entities
{
    [Table("wallet_transactions")]
    public class WalletTransaction : AuditingEntity
    {
        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; } = null!;
        public decimal Amount { get; set; }
        public WalletTransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }
        public string? Description { get; set; }
        
        // Reference to the related payment transaction if applicable
        public Guid? PaymentTransactionId { get; set; }
        [ForeignKey("PaymentTransactionId")]
        public PaymentTransaction? PaymentTransaction { get; set; }
    }
}
