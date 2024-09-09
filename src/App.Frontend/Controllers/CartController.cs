using Microsoft.AspNetCore.Mvc;

namespace App.Frontend.Controllers
{
    [Route("[controller]")]
    public class CartController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}