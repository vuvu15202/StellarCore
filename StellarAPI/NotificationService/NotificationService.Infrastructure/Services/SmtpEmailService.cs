using Microsoft.Extensions.Logging;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Services;

public class SmtpEmailService : IEmailService
{
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(ILogger<SmtpEmailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(EmailRequest request)
    {
        _logger.LogInformation("Sending Email to {To}: Subject: {Subject}, Body: {Body}", 
            request.To, request.Subject, request.Body);
            
        return Task.CompletedTask;
    }
}
