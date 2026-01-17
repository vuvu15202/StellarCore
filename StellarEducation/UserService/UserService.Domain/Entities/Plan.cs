using System;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.Domain.Enums;
using Stellar.Shared.Models;

namespace UserService.Domain.Entities
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
