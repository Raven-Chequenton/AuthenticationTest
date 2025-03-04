using AuthenticationTest.Middleware;
using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    [RoleAuthorization("Admin")]
    public IActionResult Index()
    {
        return View();
    }
}
