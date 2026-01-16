using Stellar.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CategoryService.Domain.Entities
{
    [Table("category_type")]
    public class CategoryType : AuditingEntity
    {
        [Required]
        [MaxLength(255)]
        public string Code { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Mapping { get; set; } = null!;
    }
}
