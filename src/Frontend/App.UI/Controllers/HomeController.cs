using App.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace App.UI.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
