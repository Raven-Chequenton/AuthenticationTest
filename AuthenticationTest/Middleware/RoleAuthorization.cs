using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationTest.Middleware
{
    public class RoleAuthorization : ActionFilterAttribute
    {
        private readonly string _role;
        public RoleAuthorization(string role)
        {
            _role = role;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userRole = context.HttpContext.Session.GetString("UserRole");

            if (userRole == null || userRole != _role)
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
        }
    }
}
