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
    private readonly AppHubConnectionOptions _appHubConnectionOptions;
    private readonly HubConnection _cacheNotificationHubConnection;

    public IndexModel(
        ILogger<IndexModel> logger,
        IMemoryCache memoryCache,
        HttpClient httpClient, 
        IOptions<AppHubConnectionOptions> appHubConnectionOptions)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _httpClient = httpClient;
        _appHubConnectionOptions = appHubConnectionOptions.Value;
        _cacheNotificationHubConnection = _appHubConnectionOptions.CacheNotificationHubConnection;
    }

    [BindProperty] public string CacheKey { get; set; } = string.Empty;

    public void OnGet()
    {
        bool isKeyCached = _memoryCache.TryGetValue<string>(SessionKeys.KeySent, out string? cachedValue);

        CacheKey = isKeyCached ? cachedValue!: "No value cached yet";
    }

    public async Task OnPostSendUpdate()
    {
        string cacheKey = "DefaultKey";
        if (!string.IsNullOrEmpty(CacheKey))
        {
            cacheKey = CacheKey;
        }
        string json = Json.Serialize(new { CacheKey = cacheKey });
        HttpContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        await _httpClient.PostAsync("https://localhost:44312/cache/cache-updated", httpContent)
            .ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    // Handle success
                    var s = task.Result;
                }
                else
                {
                    // Handle failure
                    var s = task.Result;
                }
            });
    }
}
