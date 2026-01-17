using Stellar.Shared.Models;
using System;

namespace NotificationService.Domain.Entities;

public class Notification : AuditingEntity
{
    public string UserId { get; set; } = string.Empty;
    public string GroupId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Type { get; set; } = "Info";
    public bool IsRead { get; set; } = false;
}
