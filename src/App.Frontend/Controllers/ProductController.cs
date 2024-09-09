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

        [HttpGet]
        public async Task<IActionResult> Index(int curentPage = 1)
        {
            PaginationDTO pagination = new();
            Response? response = await _productService.GetAsync(curentPage);
            if (response != null && response.IsSuccess)
            {
                pagination = JsonConvert.DeserializeObject<PaginationDTO>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(pagination);
        }
    }
}