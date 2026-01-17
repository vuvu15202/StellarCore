using NotificationService.Application.DTOs;
using NotificationService.Domain.Entities;
using Stellar.Shared.Services;
using System;
using System.Threading.Tasks;

namespace NotificationService.Application.Interfaces;

public interface INotificationService : IBaseService<Notification, Guid, NotificationResponse, NotificationRequest, NotificationResponse>
{
    Task SendToUserAsync(string userId, string title, string content, string type = "Info");
    Task SendToGroupAsync(string groupId, string title, string content, string type = "Info");
}
