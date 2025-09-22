using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace NetCore_SignalR.Notification.Api.Controllers;

[ApiController]
[Route("api/cache/notification")]
public class CacheNotificationController : ControllerBase
{
    private readonly IHubContext<CacheNotificationHub, ICacheNotificationHub> _hubContext;

    public CacheNotificationController(IHubContext<CacheNotificationHub, ICacheNotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpGet("greet", Name = "CacheNotificationGreet")]
    public string CacheNotificationGreet()
    {
        return "Cache Notification Hub is ready!";
    }

    [HttpPost("cacheupdated", Name = "NotifyCacheUpdated")]
    public async Task NotifyCacheUpdated([FromHeader] string cacheKey)
    {
        await _hubContext.Clients.All.CacheUpdated(cacheKey);
    }
}
