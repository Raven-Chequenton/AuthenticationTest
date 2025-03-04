using Microsoft.AspNetCore.Mvc;

namespace AuthenticationTest.Controllers
{
    public class TicketsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
