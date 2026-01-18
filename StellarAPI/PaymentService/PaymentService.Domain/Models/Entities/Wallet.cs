using System.ComponentModel.DataAnnotations.Schema;
using Stellar.Shared.Models;

namespace PaymentService.Domain.Models.Entities
{
    [Table("wallets")]
    public class Wallet : AuditingEntity
    {
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
