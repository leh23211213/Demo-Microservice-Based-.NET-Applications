
using App.Services.AuthAPI.Data;
using App.Services.ProductAPI.Models;
using App.Services.ProductAPI.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Services.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/product")]
    //[Authorize]
    public class ProductAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private Response _response;
        private IMapper _mapper;

        public ProductAPIController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _response = new Response();
            _mapper = mapper;
        }

        [HttpGet("page/{currentPage}")]
        public async Task<ActionResult<Response>> Get(int currentPage = 1)
        {
            const int pageSize = 6;
            try
            {

                var products = await _dbContext.Products
                                .Skip((currentPage - 1) * pageSize)
                                .Take(pageSize)
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

                _response.Result = _mapper.Map<PaginationDTO>(pagination);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Response>> Get(string id)
        {
            try
            {
                Product product = _dbContext.Products.First(u => u.ProductId == id);
                _response.Result = _mapper.Map<ProductDTO>(product);
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
        public async Task<ActionResult<Response>> Create(ProductDTO productDTO)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDTO);
                _dbContext.Add(product);
                _dbContext.SaveChanges();

                if (productDTO.Image != null)
                {
                    string fileName = product.ProductId + Path.GetExtension(productDTO.Image.FileName);
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
                        productDTO.Image.CopyTo(fileStream);
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
                _response.Result = _mapper.Map<ProductDTO>(product);
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
        public async Task<ActionResult<Response>> Put(ProductDTO productDTO)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDTO);
                if (productDTO.Image != null)
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
                    string fileName = product.ProductId + Path.GetExtension(productDTO.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        productDTO.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                    product.ImageLocalPath = filePath;
                }

                _dbContext.Update(product);
                _dbContext.SaveChanges();
                _response.Result = _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id}")]
        //[Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Response>> Delete(string id)
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


//         public async Task<IActionResult> Index( int page = 1)
// {
//     int pageSize = 6;
//     string cacheKey = $"Products_Page_{page}";

//     if (!_cache.TryGetValue(cacheKey, out List<ProductViewModel> productViewModels))
//     {
//         var products = await _context.Products
//             .Skip((page - 1) * pageSize)
//             .Take(pageSize)
//             .ToListAsync();

//         productViewModels = products.Select(p => new ProductViewModel
//         {
//             ProductId = p.ProductId,
//             ProductName = p.ProductName,
//             Price = p.Price,
//             Description = p.Description,
//             ImageUrl = p.ImageUrl
//         }).ToList();

//         var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));

//         _cache.Set(cacheKey, productViewModels, cacheEntryOptions);
//     }

//     var totalProduct = await _context.Products.CountAsync();
//     var totalPages = (int)Math.Ceiling(totalProduct / (double)pageSize);

//     var viewModel = new ProductListViewModel
//     {
//         Products = productViewModels,
//         CurrentPage = page,
//         TotalPages = totalPages
//     };

//     return View(viewModel);
// }