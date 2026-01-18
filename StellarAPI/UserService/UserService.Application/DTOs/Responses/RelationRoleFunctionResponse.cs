using System;

namespace UserService.Application.DTOs.Responses
{
    public class RelationRoleFunctionResponse
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Guid FunctionId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
