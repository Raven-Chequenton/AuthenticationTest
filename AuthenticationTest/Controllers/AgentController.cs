using AuthenticationTest.Middleware;
using Microsoft.AspNetCore.Mvc;

public class AgentController : Controller
{
    [RoleAuthorization("Agent")]
    public IActionResult Index()
    {
        return View();
    }
}
