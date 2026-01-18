using NotificationService.Application.DTOs;
using System.Threading.Tasks;

namespace NotificationService.Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(EmailRequest request);
}
