using Microsoft.AspNetCore.SignalR;

namespace NotificationService.API.Hubs;

public class NotificationHub : Hub
{
    // Clients can join groups (e.g., Course ID)
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}
