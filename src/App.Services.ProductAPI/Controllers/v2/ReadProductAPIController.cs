
using System.Net;
using Microsoft.AspNetCore.Mvc;
using App.Services.ProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using App.Services.ProductAPI.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
namespace App.Services.ProductAPI.Controllers
{
    [ApiVersion("2.0")]
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
