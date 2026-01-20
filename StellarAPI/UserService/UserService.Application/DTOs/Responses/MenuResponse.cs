using System.Collections.Generic;

namespace UserService.Application.DTOs.Responses
{
    public class MenuResponse
    {
        public Guid? FunctionGroupId { get; set; }
        public string? FunctionGroupName { get; set; }
        public List<string> Permissions { get; set; } = new();
        public List<FunctionResponse> Menus { get; set; } = new();
    }
}
