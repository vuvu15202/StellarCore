using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Entities;
using Stellar.Shared.APIs;
using Stellar.Shared.Services;
using System;
using System.Threading.Tasks;

namespace NotificationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : BaseApi<Notification, Guid, NotificationResponse, NotificationRequest, NotificationResponse>
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    protected override IBaseService<Notification, Guid, NotificationResponse, NotificationRequest, NotificationResponse> Service => _notificationService;

    [HttpPost("push/user/{userId}")]
    public async Task<IActionResult> SendToUser(string userId, [FromBody] NotificationRequest request)
    {
        await _notificationService.SendToUserAsync(userId, request.Title, request.Content, request.Type);
        return Ok(new { Message = "Notification sent to user" });
    }

    [HttpPost("push/group/{groupId}")]
    public async Task<IActionResult> SendToGroup(string groupId, [FromBody] NotificationRequest request)
    {
        await _notificationService.SendToGroupAsync(groupId, request.Title, request.Content, request.Type);
        return Ok(new { Message = "Notification sent to group" });
    }
}
