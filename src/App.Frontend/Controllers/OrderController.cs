using Microsoft.AspNetCore.Mvc;
using App.Frontend.Services.IServices;
using Microsoft.AspNetCore.Authorization;

namespace App.Frontend.Controllers
{

    public class OrderController : Controller
    {
        private readonly IProductService _productService;

        public OrderController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Details()
        {
            return View();
        }

        [HttpPost("OrderReadyForPickup")]
        public async Task<IActionResult> OrderReadyForPickup(int orderId)
        {
            return View();
        }

        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            return View();
        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Get(string status)
        {
            return View();
        }


    }
}