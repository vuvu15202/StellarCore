using System.IO;

namespace MediaService.Domain.Models.Values;

public class FileDownloadResult
{
    public Stream Stream { get; set; } = null!;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}
