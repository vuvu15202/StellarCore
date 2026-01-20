using System.Collections.Generic;

namespace UserService.Application.DTOs.Responses
{
    public class FunctionGroupResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<Guid>? DefaultFunctionIds { get; set; }
        public Guid? DefaultFunctionGroupId { get; set; }
        public List<Guid>? RuleIgnoreFunctionIds { get; set; }
        public bool RuleOnlyViewCreatedBy { get; set; }
        public List<Guid>? RuleViewFunctionGroupId { get; set; }
    }
}
