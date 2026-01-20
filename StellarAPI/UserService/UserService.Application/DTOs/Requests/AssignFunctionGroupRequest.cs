using System;

namespace UserService.Application.DTOs.Requests
{
    public class AssignFunctionGroupRequest
    {
        public Guid UserId { get; set; }
        public Guid FunctionGroupId { get; set; }
    }
}
