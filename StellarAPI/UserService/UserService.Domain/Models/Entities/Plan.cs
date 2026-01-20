using System;
using System.ComponentModel.DataAnnotations.Schema;
using Stellar.Shared.Models;
using UserService.Domain.Models.Enums;

namespace UserService.Domain.Models.Entities
{
    [Table("plans")]
    public class Plan : AuditingEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public PlanType Type { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
