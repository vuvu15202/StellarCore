using System;
using System.ComponentModel.DataAnnotations.Schema;
using Stellar.Shared.Models;

namespace UserService.Domain.Models.Entities
{
    [Table("relation_plan_functions")]
    public class RelationPlanFunction : AuditingEntity
    {
        public Guid PlanId { get; set; }
        public Guid FunctionId { get; set; }
    }
}
