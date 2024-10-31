
using System.Net;
using Microsoft.AspNetCore.Mvc;
using App.Services.ProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using App.Services.ProductAPI.Models;
namespace App.Services.ProductAPI.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0")]
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