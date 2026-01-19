using System;

namespace MediaService.Domain.Models.Values;

public class MediaResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public long Size { get; set; }
    public bool IsDirectory { get; set; }
    public DateTime LastModified { get; set; }
    public List<MediaResponse> Children { get; set; } = new();
}
