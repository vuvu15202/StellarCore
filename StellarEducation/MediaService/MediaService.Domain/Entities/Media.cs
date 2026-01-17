using System;

using Stellar.Shared.Models;
using System;

namespace MediaService.Domain.Entities;

public class Media : AuditingEntity
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public long Size { get; set; }
    public string Extension { get; set; } = string.Empty;
    public bool IsDirectory { get; set; }
}
