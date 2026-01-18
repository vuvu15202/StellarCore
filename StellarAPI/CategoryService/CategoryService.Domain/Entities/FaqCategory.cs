using System.ComponentModel.DataAnnotations.Schema;

namespace CategoryService.Domain.Entities
{
    [Table("faq_category")]
    public class FaqCategory : BaseCategory
    {
        public bool IsActive { get; set; }
    }
}
