using Stellar.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CategoryService.Domain.Entities
{
    public abstract class BaseCategory : AuditingEntity
    {
        [Required]
        [MaxLength(255)]
        public string Code { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = null!;

        public int? Sort { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? Path { get; set; }

        [Column("parent_id")]
        public Guid? ParentId { get; set; }

        public Guid CategoryTypeId { get; set; }

        [NotMapped]
        public List<BaseCategory> Children { get; set; } = new List<BaseCategory>();
    }
}
