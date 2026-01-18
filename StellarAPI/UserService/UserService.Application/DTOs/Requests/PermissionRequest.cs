using System;
using System.Collections.Generic;

namespace UserService.Application.DTOs.Requests
{
    public class PermissionRequest
    {
        public Guid RoleId { get; set; }
        public List<Guid> FunctionIds { get; set; } = new List<Guid>();
    }
}
