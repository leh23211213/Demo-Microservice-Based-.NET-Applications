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

        public async Task<IActionResult> Index(
                                                [FromQuery] int pageSize = 10,
                                                [FromQuery] int currentPage = 1,
                                                [FromQuery] string? search = ""
                                                )
        {
            Response? response = await _productService.Get(pageSize, currentPage, search);
            Pagination pagination = new();

            if (response.IsSuccess && response != null)
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

            Response? response = await _productService.CreateAsync(product);

            if (response.IsSuccess && response != null)
            {
                TempData["success"] = response?.Message;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(product);
        }

        public async Task<IActionResult> Update(string Id)
        {
            Response? response = await _productService.Get(Id);

            if (response.IsSuccess && response != null)
            {
                Product product = JsonConvert.DeserializeObject<Product>(Convert.ToString(response.Result));
                return View(product);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync(Product product)
        {
            Response? response = await _productService.UpdateAsync(product);

            if (response.IsSuccess && response != null)
            {
                TempData["success"] = response?.Message;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(string Id)
        {
            Response? response = await _productService.Get(Id);

            if (response.IsSuccess && response != null)
            {
                Product product = JsonConvert.DeserializeObject<Product>(Convert.ToString(response.Result));
                return View(product);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(Product product)
        {
            Response? response = await _productService.DeleteAsync(product.Id);

            if (response.IsSuccess && response != null)
            {
                TempData["success"] = response?.Message;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return RedirectToAction("Delete", "Product", new { Id = product.Id });
        }
    }
}