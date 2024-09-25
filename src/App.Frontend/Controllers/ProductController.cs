using App.Frontend.Models;
using App.Frontend.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App.Frontend.Controllers
{
    public class ProductController : Controller
    {

        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public ProductController(ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        public IActionResult Index1()
        {
            return View();
        }


        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Product product)
        {
            return View();
        }
        public async Task<IActionResult> Update(string productId)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(Product product)
        {
            return View();
        }

        public async Task<IActionResult> Delete(string productId)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string productId)
        {
            return View();
        }
    }
}