using System;
using UserService.Domain.Enums;

namespace UserService.Application.DTOs.Requests
{
    public class PlanRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public PlanType Type { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
