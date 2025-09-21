using Microsoft.AspNetCore.SignalR.Client;

namespace NetCore_SignalR.Client.A.ConfigureOptions;

public class AppHubConnectionOptions
{
    public HubConnection CacheNotificationHubConnection { get; set; }
}
