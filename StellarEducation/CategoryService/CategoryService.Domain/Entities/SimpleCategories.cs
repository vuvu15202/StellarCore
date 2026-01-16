using System.ComponentModel.DataAnnotations.Schema;

namespace CategoryService.Domain.Entities
{
    [Table("common_category")]
    public class CommonCategory : BaseCategory { }

    [Table("blood_type_category")]
    public class BloodTypeCategory : BaseCategory { }

    [Table("ethnic_category")]
    public class EthnicCategory : BaseCategory { }

    [Table("marital_status_category")]
    public class MaritalStatusCategory : BaseCategory { }

    [Table("nation_category")]
    public class NationCategory : BaseCategory { }

    [Table("relationship_category")]
    public class RelationShipCategory : BaseCategory { }

    [Table("religion_category")]
    public class ReligionCategory : BaseCategory { }

    [Table("sexual_category")]
    public class SexualCategory : BaseCategory { }

    [Table("user_manual_category")]
    public class UserManualCategory : BaseCategory 
    {
        public Guid FileId { get; set; }
    }
}
