using System;
using Stellar.Shared.Models;

namespace MediaService.Domain.Models.Entities;

public class Media : AuditingEntity
{
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Physical path on disk (e.g. "uploads/guid.ext") or relative path.
    /// </summary>
    public string StoragePath { get; set; } = string.Empty;
    public long Size { get; set; }
    public string Extension { get; set; } = string.Empty;
    public bool IsDirectory { get; set; }

    public Guid? ParentId { get; set; }

    /// <summary>
    /// Path hierarchy for easy tree retrieval (e.g. "RootID/ChildID/ThisID")
    /// </summary>
    public string Hierarchy { get; set; } = string.Empty;

    // Computed or legacy property if needed, but 'Path' was ambiguous.
    // We can keep it if it means "Virtual Path" or remove it. 
    // For now, let's keep it but map it to virtual path if possible, or deprecated.
    // The previous implementation used 'Path' as relative file system path.
    public string Path { get; set; } = string.Empty; 

    public bool IsDeleted { get; set; }
}
