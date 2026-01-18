using System;

namespace UserService.Application.DTOs.Requests
{
    public class RelationRoleFunctionRequest
    {
        public Guid RoleId { get; set; }
        public Guid FunctionId { get; set; }
    }
}
