
using System.Net;
using Microsoft.AspNetCore.Mvc;
using App.Services.ProductAPI.Models;
using App.Services.ProductAPI.Repository;


namespace App.Services.ProductAPI.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/product")]
    public class ReadProductAPIController : Controller
    {
        private Response _response;
        private readonly IReadProductRepository _readProductRepository;

        public ReadProductAPIController(
                                        IReadProductRepository readProductRepository
                                        )
        {
            _response = new Response();
            _readProductRepository = readProductRepository;
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "Default10")]
        public async Task<ActionResult<Response>> Get()
        {
            try
            {
                var products = await _readProductRepository.GetAsync();
                if (products == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Not Found";
                    _response.StatusCode = HttpStatusCode.InternalServerError;
                    return _response;
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

        /// <summary>
        /// Get detail
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ResponseCache(CacheProfileName = "Default10")]
        public async Task<ActionResult<Response>> Get(string id)
        {
            try
            {
                var product = _readProductRepository.GetAsync(id);
                if (product == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Not Found";
                    _response.StatusCode = HttpStatusCode.InternalServerError;
                    return _response;
                }

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
                products = await _readProductRepository.PaginationAsync(pageSize, currentPage, search);
                if (products == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Not Found";
                    _response.StatusCode = HttpStatusCode.InternalServerError;
                    return _response;
                }

                var totalItems = products.Count();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                if (currentPage < 1 || currentPage > totalPages)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Not Found";
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
                _response.Message = ex.Message;
                _response.StatusCode = HttpStatusCode.NotFound;
            }
            return _response;
        }
    }
}
