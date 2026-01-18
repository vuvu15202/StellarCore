using MassTransit;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;
using NotificationService.Application.IntegrationEvents;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Consumers;

public class EmailNotificationConsumer : IConsumer<ISendEmailEvent>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailNotificationConsumer> _logger;

    public EmailNotificationConsumer(IEmailService emailService, ILogger<EmailNotificationConsumer> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ISendEmailEvent> context)
    {
        _logger.LogInformation("Processing email event for: {To}", context.Message.To);

        await _emailService.SendEmailAsync(new EmailRequest
        {
            To = context.Message.To,
            Subject = context.Message.Subject,
            Body = context.Message.Body
        });
    }
}
