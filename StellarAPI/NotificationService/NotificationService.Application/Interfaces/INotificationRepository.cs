using Stellar.Shared.Interfaces.Persistence;
using NotificationService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationService.Application.Interfaces;

public interface INotificationRepository : IBasePersistence<Notification, Guid>
{
    Task<List<Notification>> GetUserNotificationsAsync(string userId);
}
