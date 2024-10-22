
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
    public class WriteProductAPIController : Controller
    {
        private Response _response;
        private readonly ApplicationDbContext _dbContext;
        // private readonly ILogger<ProductAPIController> _logger;

        public WriteProductAPIController(
                                    ApplicationDbContext dbContext
                                    // ILogger<ProductAPIController> logger
                                    )
        {
            //   _logger = logger;
            _dbContext = dbContext;
            _response = new Response();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("Create")]
        public async Task<ActionResult<Response>> Create(Product product)
        {
            try
            {
                var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
                if (existingProduct != null)
                {
                    _response.Message = "Product already exists.";
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                }
                else
                {
                    var createProduct = new Product
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Description = product.Description,
                        SizeId = product.Size.Id,
                        ColorId = product.Color.Id,
                        CategoryId = product.Category.Id,
                        BrandId = product.Brand.Id,
                    };

                    _dbContext.Products.Add(createProduct);
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
                        createProduct.ImageUrl = baseUrl + "/SmartPhone/" + fileName;
                        createProduct.ImageLocalPath = filePath;
                    }
                    else
                    {
                        createProduct.ImageUrl = "https://placehold.co/600x400";
                    }
                    _dbContext.Update(createProduct);
                    _dbContext.SaveChanges();

                    _response.Result = createProduct;
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
        [Authorize(Roles = "ADMIN")]
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
        [Authorize(Roles = "ADMIN")]
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
