namespace CategoryService.Application.DTOs
{
    public class CategoryTypeDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Mapping { get; set; } = null!;
    }

    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int? Sort { get; set; }
        public string? Description { get; set; }
        public string? Path { get; set; }
        public Guid? ParentId { get; set; }
        public Guid CategoryTypeId { get; set; }
        public bool? IsActive { get; set; }
        public List<CategoryDTO> Children { get; set; } = new();
        public CategoryDTO? Parent { get; set; }
    }
}
