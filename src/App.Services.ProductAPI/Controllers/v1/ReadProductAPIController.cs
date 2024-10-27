
using System.Net;
using Microsoft.AspNetCore.Mvc;
using App.Services.ProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using App.Services.ProductAPI.Models;
namespace App.Services.ProductAPI.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/product")]
    public class ReadProductAPIController : Controller
    {
        private Response _response;
        private readonly ApplicationDbContext _dbContext;

        public ReadProductAPIController(
                                    ApplicationDbContext dbContext
                                    )
        {
            _dbContext = dbContext;
            _response = new Response();
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "Default10")]
        public async Task<ActionResult<Response>> Get()
        {
            try
            {
                IEnumerable<Product> products = await _dbContext.Products.AsNoTracking()
                                                                        .Include(p => p.Size)
                                                                        .Include(p => p.Category)
                                                                        .Include(p => p.Color)
                                                                        .Include(p => p.Brand)
                                                                        .Select(p => new Product
                                                                        {
                                                                            Id = p.Id,
                                                                            Name = p.Name,
                                                                            Price = p.Price,
                                                                            ImageUrl = p.ImageUrl,
                                                                            ImageLocalPath = p.ImageLocalPath,
                                                                            Description = p.Description,
                                                                            Size = p.Size,
                                                                            Category = p.Category,
                                                                            Color = p.Color,
                                                                            Brand = p.Brand,
                                                                        }).ToListAsync();
                if (products == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                }
                _response.Result = products;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Result = null;
                //  _logger.LogError(ex.Message);
            }

        
            return _response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> Get(string id)
        {
            try
            {
                var product = await _dbContext.Products.AsNoTracking()
                                                        .Where(p => p.Id == id)
                                                        .Include(p => p.Category)
                                                        .Include(p => p.Size)
                                                        .Include(p => p.Color)
                                                        .Include(p => p.Brand)
                                                        .Select(p => new Product
                                                        {
                                                            Id = p.Id,
                                                            Name = p.Name,
                                                            Price = p.Price,
                                                            ImageUrl = p.ImageUrl,
                                                            ImageLocalPath = p.ImageLocalPath,
                                                            Size = p.Size,
                                                            Category = p.Category,
                                                            Color = p.Color,
                                                            Brand = p.Brand,
                                                        }).FirstOrDefaultAsync();
                if (product == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                }
                else
                {
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
        [ResponseCache(CacheProfileName = "Default10")]
        public async Task<ActionResult<Response>> Pagination(
                                                           [FromQuery] int pageSize,
                                                           [FromQuery] int currentPage,
                                                           [FromQuery] string? search = ""
                                                           )
        {
            try
            {
                IEnumerable<Product> products = null;
                products = await _dbContext.Products.AsNoTracking()
                                                .Select(p => new Product
                                                {
                                                    Id = p.Id,
                                                    Name = p.Name,
                                                    Price = p.Price,
                                                    ImageUrl = p.ImageUrl,
                                                    ImageLocalPath = p.ImageLocalPath
                                                }).ToListAsync();

                if (!string.IsNullOrEmpty(search))
                {
                    products = _dbContext.Products.Where(p => p.Name.ToLower().Contains(search.ToLower()));
                }

                Thread.Sleep(1000);

                var totalItems = products.Count();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                if (currentPage < 1 || currentPage > totalPages)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return _response;
                }

                products = products.Skip((currentPage - 1) * pageSize).Take(pageSize);
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
    }
}
