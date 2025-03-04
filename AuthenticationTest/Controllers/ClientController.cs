using AuthenticationTest.Middleware;
using Microsoft.AspNetCore.Mvc;

public class ClientController : Controller
{
    [RoleAuthorization("Client")]
    public IActionResult Index()
    {
        return View();
    }
}
