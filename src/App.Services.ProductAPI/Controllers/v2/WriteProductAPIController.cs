
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
    public class WriteProductAPIController : Controller
    {
        private IMapper _mapper;
        private Response _response;
        private readonly ApplicationDbContext _dbContext;
        // private readonly ILogger<ProductAPIController> _logger;

        public WriteProductAPIController(
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
        //[Authorize]
        // [Authorize(Roles = "ADMIN")]
        [HttpPost("Create")]
        public async Task<ActionResult<Response>> Create(Product product)
        {
            try
            {
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _response.StatusCode = HttpStatusCode.BadRequest;
            }
            return _response;
        }

        // [Authorize]
        [HttpPut("Update")]
        //[Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Response>> Update(Product product)
        {
            try
            {
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _response.StatusCode = HttpStatusCode.BadRequest;
            }
            return _response;
        }

        // [Authorize]
        [HttpDelete("Delete/{id}")]
        // [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Response>> Delete(string id)
        {
            try
            {
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
