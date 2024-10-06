using App.Frontend.Models;
using App.Frontend.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App.Frontend.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class ProductController : Controller
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index([FromQuery] string? search = null, [FromQuery] int curentPage = 1)
        {
            Pagination pagination = new();
            Response? response = await _productService.Get(search, curentPage);

            if (response.IsSuccess && response.Result != null)
            {
                pagination = JsonConvert.DeserializeObject<Pagination>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(pagination);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Product product)
        {
            if (ModelState.IsValid)
            {
                Response? response = await _productService.CreateAsync(product);
                if (response.IsSuccess && response.Result != null)
                {
                    TempData["success"] = "Product create successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(product);
        }

        public async Task<IActionResult> Update(string productId)
        {
            Response? response = await _productService.Get(productId);
            if (response.IsSuccess && response.Result != null)
            {
                Product product = JsonConvert.DeserializeObject<Product>(Convert.ToString(response.Result));
                return View(product);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync(Product product)
        {
            Response? response = await _productService.UpdateAsync(product);
            if (!response.IsSuccess && response.Result != null)
            {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(string productId)
        {
            Response? response = await _productService.Get(productId);
            if (!response.IsSuccess && response.Result != null)
            {
                Product product = JsonConvert.DeserializeObject<Product>(Convert.ToString(response.Result));
                return View(product);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(Product product)
        {
            Response? response = await _productService.DeleteAsync(product.Id);
            if (!response.IsSuccess && response.Result != null)
            {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(product);
        }
    }
}