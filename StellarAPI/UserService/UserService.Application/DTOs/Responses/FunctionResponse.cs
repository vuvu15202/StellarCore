using System;
using System.Collections.Generic;
using UserService.Application.DTOs.Requests;
using UserService.Domain.Models.Enums;

namespace UserService.Application.DTOs.Responses
{
    public class FunctionResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Path { get; set; }
        public int? SortOrder { get; set; }
        public string? SortPath { get; set; }
        public string? Icon { get; set; }
        public string? Description { get; set; }
        public string? HierarchyPath { get; set; }
        public string? Category { get; set; }
        public Guid? ParentId { get; set; }
        public bool? SuperAdmin { get; set; }
        public FunctionType Type { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public SubData? Parent { get; set; }
        public List<FunctionResponse> Children { get; set; } = new();
    }
}
