using Microsoft.AspNetCore.SignalR;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Entities;
using Stellar.Shared.Interfaces;
using Stellar.Shared.Interfaces.Persistence;
using Stellar.Shared.Models;
using Stellar.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Application.Services;

public class NotificationAppService : INotificationService
{
    private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
    private readonly INotificationRepository _notificationRepository;

    public NotificationAppService(
        IHubContext<NotificationHub, INotificationClient> hubContext,
        INotificationRepository notificationRepository)
    {
        _hubContext = hubContext;
        _notificationRepository = notificationRepository;
    }

    public ICrudPersistence<Notification, Guid> GetCrudPersistence()
    {
        return _notificationRepository;
    }

    public IGetAllPersistence<Notification> GetGetAllPersistence()
    {
        return (IGetAllPersistence<Notification>)_notificationRepository;
    }

    public void ValidateCreate(HeaderContext context, Notification entity, NotificationRequest request)
    {
    }

    public void ValidateUpdateRequest(HeaderContext context, Guid id, Notification entity, NotificationRequest request)
    {
    }

    public IQueryable<Notification> BuildFilterQuery(IQueryable<Notification> query, HeaderContext context, Dictionary<string, object> filter)
    {
        return query;
    }

    public void PostCreateHandler(HeaderContext context, Notification entity, Guid id, NotificationRequest request)
    {
        var message = ((IResponseMapper<Notification, NotificationResponse>)this).MappingResponse(context, entity);

        if (!string.IsNullOrEmpty(entity.UserId))
        {
            _hubContext.Clients.Group($"User_{entity.UserId}").ReceiveNotification(message);
        }
        else if (!string.IsNullOrEmpty(entity.GroupId))
        {
            _hubContext.Clients.Group(entity.GroupId).ReceiveNotification(message);
        }
    }

    public async Task SendToUserAsync(string userId, string title, string content, string type = "Info")
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Content = content,
            Type = type
        };

        _notificationRepository.Save(notification);

        var message = new NotificationResponse
        {
            Id = notification.Id,
            UserId = userId,
            Title = title,
            Content = content,
            Type = type,
            CreatedAt = notification.CreatedAt
        };

        await _hubContext.Clients.Group($"User_{userId}").ReceiveNotification(message);
    }

    public async Task SendToGroupAsync(string groupId, string title, string content, string type = "Info")
    {
        var notification = new Notification
        {
            GroupId = groupId,
            Title = title,
            Content = content,
            Type = type
        };

        _notificationRepository.Save(notification);

        var message = new NotificationResponse
        {
            Id = notification.Id,
            GroupId = groupId,
            Title = title,
            Content = content,
            Type = type,
            CreatedAt = notification.CreatedAt
        };

        await _hubContext.Clients.Group(groupId).ReceiveNotification(message);
    }
}

public class NotificationHub : Hub<INotificationClient>
{
    public async Task SubscribeToUser(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
    }

    public async Task SubscribeToGroup(string groupId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
    }

    public async Task UnsubscribeFromGroup(string groupId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
    }
}
