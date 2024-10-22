
using System.Net;
using Microsoft.AspNetCore.Mvc;
using App.Services.ProductAPI.Data;
using App.Services.ProductAPI.Models;
using Microsoft.AspNetCore.Authorization;
namespace App.Services.ProductAPI.Controllers
{
    [ApiVersion("2.0")]
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
