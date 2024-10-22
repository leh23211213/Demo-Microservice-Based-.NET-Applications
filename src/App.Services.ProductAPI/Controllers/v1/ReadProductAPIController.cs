
using System.Net;
using Microsoft.AspNetCore.Mvc;
using App.Services.ProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using App.Services.ProductAPI.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
namespace App.Services.ProductAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/product")]
    //[Route("api/v{major:apiVersion}.{minor:apiVersion}.{patch:int}/products")]
    public class ReadProductAPIController : Controller
    {
        private IMapper _mapper;
        private Response _response;
        private readonly ApplicationDbContext _dbContext;
        // private readonly ILogger<ProductAPIController> _logger;

        public ReadProductAPIController(
                                    IMapper mapper,
                                    ApplicationDbContext dbContext
                                    // ILogger<ProductAPIController> logger
                                    )
        {
            //   _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
            _response = new Response();
        }

        // [Authorize]
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
                if (products == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                }
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
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

        // [Authorize]
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
                    _response.Result = _mapper.Map<ProductDetailsDto>(product); ;
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

        //[Authorize]
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

                if (currentPage < 1 || currentPage > totalPages)
                {
                    return BadRequest("Not Found");
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

                IEnumerable<PaginateProduct> paginateProduct = _mapper.Map<IEnumerable<PaginateProduct>>(products);

                Pagination pagination = new()
                {
                    Products = paginateProduct,
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
