using App.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Frontend.Extensions
{
    public class AuthExceptionRedirection : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is AuthException)
                context.Result = new RedirectToActionResult("Login", "Login", new { area = "Account" });
        }
    }
}
