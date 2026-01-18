using System;
using System.Collections.Generic;

namespace UserService.Application.DTOs.Requests
{
    public class PlanPermissionRequest
    {
        public Guid PlanId { get; set; }
        public List<Guid> FunctionIds { get; set; } = new List<Guid>();
    }
}
