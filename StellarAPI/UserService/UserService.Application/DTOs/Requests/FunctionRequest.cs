using System;
using System.ComponentModel.DataAnnotations;
using UserService.Domain.Enums;

namespace UserService.Application.DTOs.Requests
{
    public class FunctionRequest
    {
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = null!;

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        public string? Path { get; set; }

        public int? SortOrder { get; set; }

        public string? SortPath { get; set; }

        public string? Icon { get; set; }

        public string? Description { get; set; }

        public Guid? ParentId { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public FunctionType Type { get; set; }
    }

    public class SubData
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}
