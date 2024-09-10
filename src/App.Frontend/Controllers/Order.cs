using Microsoft.AspNetCore.Mvc;
using App.Frontend.Models;

namespace App.Frontend.Controllers
{

    public class OrderController : Controller
    {
        private readonly ILogger<Order> _logger;

        public OrderController(ILogger<Order> logger)
        {
            _logger = logger;
        }

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