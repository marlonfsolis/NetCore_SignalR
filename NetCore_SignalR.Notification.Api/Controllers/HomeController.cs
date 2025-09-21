using Microsoft.AspNetCore.Mvc;

namespace NetCore_SignalR.Notification.Api.Controllers;

[ApiController]
[Route("/")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return RedirectPermanent("/api/cache/notification/greet");
    }
}
