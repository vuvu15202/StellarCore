using System;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.Domain.Enums;
using Stellar.Shared.Models;

namespace UserService.Domain.Entities
{
    [Table("functions")]
    public class Function : AuditingEntity
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Path { get; set; }
        public int? SortOrder { get; set; }
        public string? SortPath { get; set; }
        public string? Icon { get; set; }
        public string? Description { get; set; }
        public string? HierarchyPath { get; set; }
        public string? Category { get; set; }
        public Guid? ParentId { get; set; }
        public FunctionType Type { get; set; }
    }
}
