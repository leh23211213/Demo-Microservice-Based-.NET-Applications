using App.Domain.Admin.Models;
using App.Domain.Admin.Services.IServices;
using App.Domain.Admin.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace App.Domain.Admin.Controllers
{
<<<<<<< HEAD

    [Authorize(Roles = "ADMIN")]
=======
<<<<<<<< HEAD:src/domain/App.Frontend/Controllers/ProductController.cs

    [Authorize(Roles = "ADMIN")]
========
>>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/domain/App.Domain.Admin/Controllers/ProductController.cs
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
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
<<<<<<< HEAD
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
            var categoryList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Category , Value = StaticDetail.Category},
            };
            var brandList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Brand , Value = StaticDetail.Brand},
            };
            var colorList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Color , Value = StaticDetail.Color},
            };
            var sizeList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Size , Value = StaticDetail.Size},
            };
            ViewBag.CategoryList = categoryList;
            ViewBag.BrandList = brandList;
            ViewBag.ColorList = colorList;
            ViewBag.SizeList = sizeList;
            return View();
=======
<<<<<<<< HEAD:src/domain/App.Frontend/Controllers/ProductController.cs
            Response? response = await _productService.Get(pageSize, currentPage, search);
            Pagination pagination = new();
            if (response.IsSuccess && response != null)
========
            if (User.Identity.IsAuthenticated)
>>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/domain/App.Domain.Admin/Controllers/ProductController.cs
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
            else
            {
                return RedirectToAction("Login", "Authentication", new { area = "Account" });
            }
<<<<<<<< HEAD:src/domain/App.Frontend/Controllers/ProductController.cs
            return View(pagination);
========
>>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/domain/App.Domain.Admin/Controllers/ProductController.cs
        }

        public async Task<IActionResult> Create()
        {
            var categoryList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Category , Value = StaticDetail.Category},
            };
            var brandList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Brand , Value = StaticDetail.Brand},
            };
            var colorList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Color , Value = StaticDetail.Color},
            };
            var sizeList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Size , Value = StaticDetail.Size},
            };
                ViewBag.CategoryList = categoryList;
                ViewBag.BrandList = brandList;
                ViewBag.ColorList = colorList;
                ViewBag.SizeList = sizeList;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Authentication", new { area = "Account" });
            }
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Product product)
        {
            product.Size.Id = product.Size.RAM == "128GB" ? 1 : 0;
            product.Color.Id = product.Color.Name == "Black" ? 1 : 0;
            product.Category.Id = product.Category.Name == "Smartphone" ? 1 : 0;
            product.Brand.Id = product.Brand.Name == "Apple" ? 1 : 0;

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

            var categoryList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Category , Value = StaticDetail.Category},
            };
            var brandList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Brand , Value = StaticDetail.Brand},
            };
            var colorList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Color , Value = StaticDetail.Color},
            };
            var sizeList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Size , Value = StaticDetail.Size},
            };
            ViewBag.CategoryList = categoryList;
            ViewBag.BrandList = brandList;
            ViewBag.ColorList = colorList;
            ViewBag.SizeList = sizeList;

            return View(product);
        }

        public async Task<IActionResult> Update(string Id)
        {
<<<<<<< HEAD
            var categoryList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Category , Value = StaticDetail.Category},
            };
            var brandList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Brand , Value = StaticDetail.Brand},
            };
            var colorList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Color , Value = StaticDetail.Color},
            };
            var sizeList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Size , Value = StaticDetail.Size},
            };
            ViewBag.CategoryList = categoryList;
            ViewBag.BrandList = brandList;
            ViewBag.ColorList = colorList;
            ViewBag.SizeList = sizeList;

=======
            if (User.Identity.IsAuthenticated)
            {
                var categoryList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Category , Value = StaticDetail.Category},
            };
            var brandList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Brand , Value = StaticDetail.Brand},
            };
            var colorList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Color , Value = StaticDetail.Color},
            };
            var sizeList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Size , Value = StaticDetail.Size},
            };
            ViewBag.CategoryList = categoryList;
            ViewBag.BrandList = brandList;
            ViewBag.ColorList = colorList;
            ViewBag.SizeList = sizeList;

<<<<<<<< HEAD:src/domain/App.Frontend/Controllers/ProductController.cs
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
            Response? response = await _productService.Get(Id);
            if (response.IsSuccess && response != null)
            {
                Product product = JsonConvert.DeserializeObject<Product>(Convert.ToString(response.Result));
                return View(product);
<<<<<<< HEAD
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return RedirectToAction(nameof(Index));
=======
========
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

>>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/domain/App.Domain.Admin/Controllers/ProductController.cs
            }
            else
            {
                return RedirectToAction("Login", "Authentication", new { area = "Account" });
            }
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync(Product product)
        {
            product.Size.Id = product.Size.RAM == "128GB" ? 1 : 0;
            product.Color.Id = product.Color.Name == "Black" ? 1 : 0;
            product.Category.Id = product.Category.Name == "Smartphone" ? 1 : 0;
            product.Brand.Id = product.Brand.Name == "Apple" ? 1 : 0;

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

            var categoryList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Category , Value = StaticDetail.Category},
            };
            var brandList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Brand , Value = StaticDetail.Brand},
            };
            var colorList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Color , Value = StaticDetail.Color},
            };
            var sizeList = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.Size , Value = StaticDetail.Size},
            };
            ViewBag.CategoryList = categoryList;
            ViewBag.BrandList = brandList;
            ViewBag.ColorList = colorList;
            ViewBag.SizeList = sizeList;

            return View(product);
        }

        public async Task<IActionResult> Delete(string Id)
        {
<<<<<<< HEAD
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
=======
<<<<<<<< HEAD:src/domain/App.Frontend/Controllers/ProductController.cs
            Response? response = await _productService.Get(Id);
            if (response.IsSuccess && response != null)
========
            if (User.Identity.IsAuthenticated)
>>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/domain/App.Domain.Admin/Controllers/ProductController.cs
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
            else
            {
                return RedirectToAction("Login", "Authentication", new { area = "Account" });
            }
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
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