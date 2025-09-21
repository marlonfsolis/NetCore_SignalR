namespace NetCore_SignalR.Notification.Api.Hubs;

public interface ICacheNotificationHub
{
   // Define methods that clients can call
    Task CacheUpdated(string cacheKey);
    //Task NotifyCacheCleared(string cacheKey);
    //Task NotifyCacheError(string cacheKey, string errorMessage);
}
