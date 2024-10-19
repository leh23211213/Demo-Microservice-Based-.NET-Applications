
using System.Net;
using Microsoft.AspNetCore.Mvc;
using App.Services.ProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using App.Services.ProductAPI.Models;
using Microsoft.AspNetCore.Authorization;
namespace App.Services.ProductAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/product")]
    //[Route("api/v{major:apiVersion}.{minor:apiVersion}.{patch:int}/products")]
    public class ProductAPIController : Controller
    {
        private Response _response;
        private readonly ApplicationDbContext _dbContext;
        // private readonly ILogger<ProductAPIController> _logger;

        public ProductAPIController(
                                    ApplicationDbContext dbContext
                                    // ILogger<ProductAPIController> logger
                                    )
        {
            //   _logger = logger;
            _dbContext = dbContext;
            _response = new Response();
        }

        [HttpGet]
        public async Task<ActionResult<Response>> Get()
        {
            // _logger.LogInformation("orther service call");
            try
            {
                IEnumerable<Product> products = await _dbContext.Products
                                                            .Include(p => p.Category)
                                                            .Include(p => p.Size)
                                                            .Include(p => p.Color)
                                                            .Include(p => p.Brand)
                                                            .ToListAsync();
                _response.Result = products;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _response.StatusCode = HttpStatusCode.NotFound;
                //  _logger.LogError(ex.Message);
            }
            return _response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> Get(string id)
        {
            try
            {
                var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);
                if (product == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                }
                else
                {
                    product = await _dbContext.Products
                                .Where(p => p.Id == id)
                                .Include(p => p.Category)
                                .Include(p => p.Size)
                                .Include(p => p.Color)
                                .Include(p => p.Brand)
                                .FirstOrDefaultAsync();

                    _response.Result = product;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _response.StatusCode = HttpStatusCode.NotFound;
            }
            return _response;
        }

        [HttpGet("Pagination")]
        // [ResponseCache(CacheProfileName = "Default10")]
        public async Task<ActionResult<Response>> Pagination(
                                                    [FromQuery] int pageSize,
                                                    [FromQuery] int currentPage,
                                                    [FromQuery] string? search = ""
                                                    )
        {
            try
            {
                IEnumerable<Product> products = null;
                products = await _dbContext.Products.ToListAsync();

                if (!string.IsNullOrEmpty(search))
                {
                    products = _dbContext.Products.Where(p => p.Name.ToLower().Contains(search.ToLower()));
                }

                var totalItems = products.Count();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                if (currentPage < 0)
                {
                    return BadRequest("Not Found");
                }
                else
                {
                    if (currentPage > totalPages)
                    {
                        currentPage = totalPages;
                    }
                    products = products.Skip((currentPage - 1) * pageSize).Take(pageSize)
                                                    .Select(p => new Product
                                                    {
                                                        Id = p.Id,
                                                        Name = p.Name,
                                                        Price = p.Price,
                                                        ImageUrl = p.ImageUrl,
                                                        ImageLocalPath = p.ImageLocalPath
                                                    });
                }

                Pagination pagination = new()
                {
                    Products = products,
                    totalPages = totalPages,
                    currentPage = currentPage
                };
                _response.Result = pagination;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Message = ex.Message;
            }
            return _response;
        }

        // [Authorize(Roles = "ADMIN")]
        [HttpPost("Create")]
        public async Task<ActionResult<Response>> Create(Product product)
        {
            try
            {
                var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
                if (existingProduct != null)
                {
                    // Nếu sản phẩm đã tồn tại, trả về thông báo hoặc xử lý lỗi
                    _response.Message = "Product already exists.";
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                }
                else
                {
                    // // Xử lý thêm sản phẩm, cho phép các trường Size, Color, Brand và Category nullable
                    // if (ModelState.IsValid)
                    // {
                    //     product.Id = Guid.NewGuid().ToString();

                    //     // Nếu người dùng không chọn, các thuộc tính này sẽ là null
                    //     product.Size = product.Size?.Id != null ? context.Sizes.Find(product.Size.Id) : null;
                    //     product.Color = product.Color?.Id != null ? context.Colors.Find(product.Color.Id) : null;
                    //     product.Brand = product.Brand?.Id != null ? context.Brands.Find(product.Brand.Id) : null;
                    //     product.Category = product.Category?.Id != null ? context.Categories.Find(product.Category.Id) : null;

                    //     context.Products.Add(product);
                    //     context.SaveChanges();
                    //     return RedirectToAction("Index");
                    // }

                    _dbContext.Add(product);
                    _dbContext.SaveChanges();

                    if (product.Image != null)
                    {
                        string fileName = product.Id + Path.GetExtension(product.Image.FileName);
                        string filePath = @"wwwroot\lib\Product\SmartPhone\" + fileName;

                        var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                        FileInfo fileInfo = new FileInfo(directoryLocation);
                        if (fileInfo.Exists)
                        {
                            fileInfo.Delete();
                        }

                        var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                        using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                        {
                            product.Image.CopyTo(fileStream);
                        }
                        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                        product.ImageUrl = baseUrl + "/SmartPhone/" + fileName;
                        product.ImageLocalPath = filePath;
                    }
                    else
                    {
                        product.ImageUrl = "https://placehold.co/600x400";
                    }
                    _dbContext.Update(product);
                    _dbContext.SaveChanges();

                    _response.Result = product;
                    _response.Message = "Create product successfully";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _response.StatusCode = HttpStatusCode.BadRequest;
            }
            return _response;
        }

        [HttpPut("Update")]
        // [Authorize(Roles = "ADMIN")]

        public async Task<ActionResult<Response>> Update(Product product)
        {
            try
            {
                Product p = _dbContext.Products.FirstOrDefault(u => u.Id == product.Id);
                if (p is null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                }
                else
                {

                    if (product.Image != null)
                    {
                        if (!string.IsNullOrEmpty(product.ImageLocalPath))
                        {
                            var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                            FileInfo file = new FileInfo(oldFilePathDirectory);
                            if (file.Exists)
                            {
                                file.Delete();
                            }
                        }
                        string fileName = product.Id + Path.GetExtension(product.Image.FileName);
                        string filePath = @"wwwroot\ProductImages\" + fileName;
                        var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                        using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                        {
                            product.Image.CopyTo(fileStream);
                        }
                        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                        product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                        product.ImageLocalPath = filePath;
                    }

                    _dbContext.Update(product);
                    _dbContext.SaveChanges();

                    _response.Message = "Product update successfully";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _response.StatusCode = HttpStatusCode.BadRequest;
            }
            return _response;
        }

        [HttpDelete("Delete/{id}")]
        //  [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Response>> Delete(string id)
        {
            try
            {
                Product product = _dbContext.Products.FirstOrDefault(u => u.Id == id);
                if (product == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                }
                else
                {
                    if (!string.IsNullOrEmpty(product.ImageLocalPath))
                    {
                        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                        FileInfo file = new FileInfo(oldFilePathDirectory);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    _dbContext.Remove(product);
                    _dbContext.SaveChanges();

                    _response.Message = "Product deleted successfully";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _response.StatusCode = HttpStatusCode.BadRequest;
            }
            return _response;
        }
    }
}
