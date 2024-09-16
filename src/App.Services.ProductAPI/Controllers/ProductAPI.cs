
using App.Services.ProductAPI.Data;
using App.Services.ProductAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Services.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/product")]
    //[Authorize]
    public class ProductAPIController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private Response _response;

        public ProductAPIController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _response = new Response();
        }

        [HttpGet("page/{currentPage}")]
        public async Task<ActionResult<Response>> GetAsync(int currentPage = 1)
        {
            const int pageSize = 6;
            IEnumerable<Product> products;
            try
            {
                products = await _dbContext.Products.Skip((currentPage - 1) * pageSize).Take(pageSize)
                                .Select(p => new Product
                                {
                                    ProductName = p.ProductName,
                                    Price = p.Price,
                                    ImageUrl = p.ImageUrl,
                                    ImageLocalPath = p.ImageLocalPath
                                }).ToListAsync();

                var totalItems = await _dbContext.Products.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                Pagination pagination = new()
                {
                    Products = products,
                    totalPages = totalPages,
                    currentPage = currentPage,
                };

                if (currentPage > totalPages)
                {
                    currentPage = totalPages;
                }

                _response.Result = pagination;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> GetAsync(string id)
        {
            try
            {
                Product product = _dbContext.Products.First(u => u.ProductId == id);
                _response.Result = product;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        //[Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Response>> CreateAsync(Product product)
        {
            try
            {
                _dbContext.Add(product);
                _dbContext.SaveChanges();

                if (product.Image != null)
                {
                    string fileName = product.ProductId + Path.GetExtension(product.Image.FileName);
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
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        //[Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Response>> UpdateAsync(Product product)
        {
            try
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
                    string fileName = product.ProductId + Path.GetExtension(product.Image.FileName);
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
                _response.Result = product;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Response>> RemoveAsync(string id)
        {
            try
            {
                Product product = _dbContext.Products.First(u => u.ProductId == id);
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
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
