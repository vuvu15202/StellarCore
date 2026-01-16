using System.Collections.Generic;

namespace UserService.Application.DTOs.Responses
{
    public class MenuResponse
    {
        public List<string> Permissions { get; set; } = new();
        public List<FunctionResponse> Menus { get; set; } = new();
    }
}
