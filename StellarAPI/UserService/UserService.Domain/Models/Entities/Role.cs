using System;
using Stellar.Shared.Models;

namespace UserService.Domain.Models.Entities
{
    public class Role : AuditingEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
