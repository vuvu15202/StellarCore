using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Stellar.Shared.Models;

namespace UserService.Domain.Entities
{
    [Table("function_groups")]
    public class FunctionGroup : AuditingEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        
        /// <summary>
        /// List of Function IDs
        /// </summary>
        public List<Guid>? DefaultFunctionIds { get; set; }

        /// <summary>
        /// Default FunctionGroupId for newly created accounts
        /// </summary>
        public Guid? DefaultFunctionGroupId { get; set; }

        /// <summary>
        /// List of restricted Function IDs
        /// </summary>
        public List<Guid>? RuleIgnoreFunctionIds { get; set; }

        /// <summary>
        /// If true, user can only view records created by themselves
        /// </summary>
        public bool RuleOnlyViewCreatedBy { get; set; }

        /// <summary>
        /// List of FunctionGroup IDs permitted to be assigned to others
        /// </summary>
        public List<Guid>? RuleViewFunctionGroupId { get; set; }
    }
}
