using System.ComponentModel.DataAnnotations.Schema;

namespace CategoryService.Domain.Entities
{
    [Table("province_category")]
    public class ProvinceCategory : BaseCategory
    {
        public bool IsActive { get; set; }
    }

    [Table("district_category")]
    public class DistrictCategory : BaseCategory
    {
        public bool IsActive { get; set; }
    }

    [Table("ward_category")]
    public class WardCategory : BaseCategory
    {
        public bool IsActive { get; set; }
    }
}
