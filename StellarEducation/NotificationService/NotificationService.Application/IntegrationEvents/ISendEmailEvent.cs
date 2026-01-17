namespace NotificationService.Application.IntegrationEvents;

public interface ISendEmailEvent
{
    string To { get; }
    string Subject { get; }
    string Body { get; }
}
