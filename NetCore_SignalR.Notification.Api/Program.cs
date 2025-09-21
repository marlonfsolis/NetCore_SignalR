

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSignalR();

var app = builder.Build();

app.MapControllers();

app.MapHub<CacheNotificationHub>(CacheNotificationHub.HubUrl);

await app.RunAsync();
