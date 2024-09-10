using App.Frontend.Models;
using App.Frontend.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App.Frontend.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(int curentPage)
        {
            Pagination pagination = new();
            Response? response = await _productService.GetAsync(curentPage);
            if (response != null && response.IsSuccess)
            {
                pagination = JsonConvert.DeserializeObject<Pagination>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(pagination);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Detail(string id)
        {
            Product product = new();
            Response? response = await _productService.GetAsync(id);
            if (response != null && response.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<Product>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            return View();
        }
        public async Task<IActionResult> Update()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            return View();
        }
    }
}