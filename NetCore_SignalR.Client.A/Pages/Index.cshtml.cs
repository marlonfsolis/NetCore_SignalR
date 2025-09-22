using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace NetCore_SignalR.Client.A.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly HttpClient _httpClient;
    private readonly SignalROptions _signalROptions;
    private readonly AppHubConnectionOptions _appHubConnectionOptions;
    private readonly HubConnection _cacheNotificationHubConnection;

    public IndexModel(
        ILogger<IndexModel> logger,
        IMemoryCache memoryCache,
        HttpClient httpClient, 
        IOptions<AppHubConnectionOptions> appHubConnectionOptions,
        IOptions<SignalROptions> signalROptions)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _httpClient = httpClient;
        _signalROptions = signalROptions.Value;
        _appHubConnectionOptions = appHubConnectionOptions.Value;
        _cacheNotificationHubConnection = _appHubConnectionOptions.CacheNotificationHubConnection;
    }

    [BindProperty] public string CacheKey { get; set; } = string.Empty;

    public void OnGet()
    {
        // Here we read from the in-memory cache what is saved on the event triggered by the SignalR notification
        // This event is registered in UseNotificationHubEventsExtension.

        bool isKeyCached = _memoryCache.TryGetValue<string>(SessionKeys.KeySent, out string? cachedValue);
        CacheKey = isKeyCached ? cachedValue! : "No value cached yet";
    }

    public async Task OnPostSendUpdate()
    {
        ModelState.Clear();

        if (string.IsNullOrEmpty(CacheKey))
        {
            ModelState.AddModelError(string.Empty, $"Cache key cannot be null.");
            return;
        }

        HttpContent httpContent = new StringContent(string.Empty);
        httpContent.Headers.Add("cacheKey", CacheKey);

        await _httpClient.PostAsync($"{_signalROptions.NotificationHubBaseUrl}/api/cache/notification/cacheupdated", httpContent)
            .ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully && task.Result.IsSuccessStatusCode)
                {
                    ViewData["SuccessMessage"] = $"Cache update notification sent successfully for key: {CacheKey}." +
                    $" Please refresh to see if the value persist.";
                }
                else
                {
                    // Handle failure
                    var result = task.Result;
                    ModelState.AddModelError(string.Empty, $"Something happend while sending the request to [cacheupdated]. " +
                        $"Status Code: {result.ReasonPhrase}. Reason Phrase: {result.ReasonPhrase}");
                }
            });
    }

    public async Task OnPostInvokeUpdated()
    {
        ModelState.Clear();

        if (string.IsNullOrEmpty(CacheKey))
        {
            ModelState.AddModelError(string.Empty, $"Cache key cannot be null.");
            return;
        }

        await _cacheNotificationHubConnection.InvokeAsync("NotifyCacheUpdated", CacheKey)
            .ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    ViewData["SuccessMessage"] = $"Cache update notification sent successfully for key: {CacheKey}." +
                    $" Please refresh to see if the value persist.";
                }
                else
                {
                    // Handle failure
                    var exception = task.Exception;
                    ModelState.AddModelError(string.Empty, $"Something happend while sending the request to [CacheUpdated] via SignalR. " +
                        $"Exception: {exception?.GetBaseException().Message}");
                }
            });
    }
}
