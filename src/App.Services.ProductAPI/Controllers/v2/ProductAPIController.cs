
using System.Net;
using App.Services.ProductAPI.Data;
using App.Services.ProductAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace App.Services.ProductAPI.Controllers.v2
{
    //[Route("api/v{major:apiVersion}.{minor:apiVersion}.{patch:int}/products")]
    [Route("api/v{version:apiVersion}/product")]
    [ApiController]
    [ApiVersion("2.0")]
    public class ProductAPIController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private Response _response;

        public ProductAPIController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _response = new Response();
        }

        [HttpGet("GetAllProduct")]
        public async Task<ActionResult<Response>> GetAllProduct()
        {
            try
            {
                IEnumerable<Product> product = await _dbContext.Products
                                                            // .Include(p => p.Category)
                                                            // .Include(p => p.Size)
                                                            // .Include(p => p.Color)
                                                            // .Include(p => p.Brand)
                                                            .ToListAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = product;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _response.StatusCode = HttpStatusCode.NotFound;
            }
            return _response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> Get(string id)
        {
            try
            {
                var product = await _dbContext.Products
                                                // .Include(p => p.Category)
                                                // .Include(p => p.Size)
                                                // .Include(p => p.Color)
                                                // .Include(p => p.Brand)
                                                .FirstOrDefaultAsync(p => p.Id == id);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = product;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _response.StatusCode = HttpStatusCode.NotFound;
            }
            return _response;
        }

        [HttpGet]
        // [ResponseCache(CacheProfileName = "Default10")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response>> Get(
                                                    [FromQuery] int pageSize = 6,
                                                    [FromQuery] int currentPage = 1,
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

                _response.StatusCode = HttpStatusCode.OK;
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

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Response>> Create(Product product)
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Response>> Update(Product product)
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
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Response>> Remove(string id)
        {
            try
            {
                Product product = _dbContext.Products.First(u => u.Id == id);
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
