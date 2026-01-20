using System;

namespace UserService.Application.DTOs.Requests
{
    public class RoleRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
