
using System.Text.Json;
using App.Services.ProductAPI.Data;
using App.Services.ProductAPI.Models;
using App.Services.ProductAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Services.ProductAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}/product")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProductAPIController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private Response _response;

        public ProductAPIController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _response = new Response();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response>> Get()
        {
            try
            {
                IEnumerable<Product> products = await _dbContext.Products.ToListAsync();
                _response.Result = products;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            }
            return _response;
        }


        [HttpGet("page/{currentPage}")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Response>> GetAsync([FromQuery] string? search, int currentPage = 1)
        {
            const int pageSize = 6;
            IEnumerable<Product> products;
            try
            {
                products = await _dbContext.Products.Skip((currentPage - 1) * pageSize).Take(pageSize)
                                .Select(p => new Product
                                {
                                    Name = p.Name,
                                    Price = p.Price,
                                    ImageUrl = p.ImageUrl,
                                    ImageLocalPath = p.ImageLocalPath
                                }).AsNoTracking().ToListAsync();

                var totalItems = await _dbContext.Products.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                Pagination pagination = new()
                {
                    Products = products,
                    totalPages = totalPages,
                    currentPage = currentPage,
                };

                if (currentPage > 0)
                {
                    if (currentPage > totalPages)
                    {
                        currentPage = totalPages;
                    }
                }
                _response.StatusCode = System.Net.HttpStatusCode.OK;
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
                Product product = _dbContext.Products.First(u => u.Id == id);
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
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Response>> CreateAsync(Product product)
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
        public async Task<ActionResult<Response>> RemoveAsync(string id)
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
