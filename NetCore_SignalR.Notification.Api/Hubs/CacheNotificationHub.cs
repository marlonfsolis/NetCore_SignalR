using Microsoft.AspNetCore.SignalR;

namespace NetCore_SignalR.Notification.Api.Hubs;

public class CacheNotificationHub : Hub<ICacheNotificationHub>
{
    public static string HubName => "CacheNotificationHub";
    public static string HubUrl => $"/{HubName}";

    public Task NotifyCacheUpdated(string cacheKey)
    {
        return Clients.All.CacheUpdated(cacheKey);
    }

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
}
