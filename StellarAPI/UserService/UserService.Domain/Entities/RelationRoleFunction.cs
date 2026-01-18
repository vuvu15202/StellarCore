using System;
using Stellar.Shared.Models;

namespace UserService.Domain.Entities
{
    public class RelationRoleFunction : AuditingEntity
    {
        public Guid RoleId { get; set; }
        public Guid FunctionId { get; set; }

        // Navigation properties (optional, depending on if we want full object graph)
        // For now, keeping it simple as a join table with IDs
    }
}
