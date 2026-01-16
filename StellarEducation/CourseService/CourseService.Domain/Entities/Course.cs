using System;
using Stellar.Shared.Models;

namespace CourseService.Domain.Entities
{
    public class Course : AuditingEntity
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
