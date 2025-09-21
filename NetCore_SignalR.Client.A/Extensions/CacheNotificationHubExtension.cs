using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Caching.Memory;

namespace NetCore_SignalR.Client.A.Extensions;

public static class CacheNotificationHubExtension
{
    public static IServiceCollection AddCacheNotificationHub(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var connectionHub = new HubConnectionBuilder()
        .WithUrl(builder.Configuration["SignalR:CacheHubUrl"] ?? string.Empty)
        .WithAutomaticReconnect()
        .Build();

        builder.Services.Configure<AppHubConnectionOptions>(options => options.CacheNotificationHubConnection = connectionHub);

        connectionHub.StartAsync().GetAwaiter().GetResult();

        // This can go in any other place where we want to listen to cache update notifications
        // We would typically put this in a service that needs to respond to cache updates
        //  And can access the HubConnection via dependency injection (AppHubConnectionOptions.CacheNotificationHubConnection)
        connectionHub.On<string>("CacheUpdated", (key) =>
        {
            // Handle cache update notifications here
            Debug.WriteLine($"Cache updated: {key}");

            
            IMemoryCache? memoryCache = builder.Services.BuildServiceProvider().GetService<IMemoryCache>();
            if (memoryCache == null) return;

            memoryCache?.CreateEntry("KeySent")?.SetValue(key);
        });

        return services;
    }
}
