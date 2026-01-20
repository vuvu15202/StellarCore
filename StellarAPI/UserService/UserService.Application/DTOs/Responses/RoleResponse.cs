using System;

namespace UserService.Application.DTOs.Responses
{
    public class RoleResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
