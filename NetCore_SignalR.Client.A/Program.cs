using NetCore_SignalR.Client.A.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Enable static web assets in the "Development.Docker" environment
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-9.0
// Check the section "Static files in non-Development environments"
if (builder.Environment.IsEnvironment("Development.Docker"))
{
    builder.WebHost.UseStaticWebAssets();
}

builder.Services.AddRazorPages();

builder.Services.AddMemoryCache();

builder.Services.AddSignalR();

builder.Services.AddCacheNotificationHub(builder);

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();

app.MapStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
