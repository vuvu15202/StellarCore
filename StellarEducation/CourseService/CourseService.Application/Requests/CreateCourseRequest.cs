using System;

namespace CourseService.Application.Requests
{
    public class CreateCourseRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
