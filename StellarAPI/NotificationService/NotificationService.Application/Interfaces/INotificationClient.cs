using NotificationService.Application.DTOs;
using System.Threading.Tasks;

namespace NotificationService.Application.Interfaces;

public interface INotificationClient
{
    Task ReceiveNotification(NotificationResponse notification);
}
