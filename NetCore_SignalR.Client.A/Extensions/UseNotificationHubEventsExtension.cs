using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace NetCore_SignalR.Client.A.Extensions;

public static class UseNotificationHubEventsExtension
{
    public static IApplicationBuilder UseNotificationHubEvents(this WebApplication app)
    {
        var appHubConnectionOptions = app.Services.GetRequiredService<IOptions<AppHubConnectionOptions>>().Value;
        var connectionHub = appHubConnectionOptions.CacheNotificationHubConnection;
        if (connectionHub == null) throw new ArgumentNullException(nameof(connectionHub), "CacheNotificationHubConnection is not configured.");
        connectionHub.On<string>("CacheUpdated", (key) =>
        {
            // Handle cache update notifications here
            Debug.WriteLine($"Cache updated: {key}");

            using (var scope = app.Services.CreateScope())
            {
                var memoryCache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
                memoryCache.Set(SessionKeys.KeySent, key);
            }
        });
        return app;
    }
}
