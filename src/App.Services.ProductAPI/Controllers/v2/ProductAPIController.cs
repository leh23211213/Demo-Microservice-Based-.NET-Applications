
using System.Net;
using App.Services.ProductAPI.Data;
using App.Services.ProductAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace App.Services.ProductAPI.Controllers.v2
{
    //[Route("api/v{major:apiVersion}.{minor:apiVersion}.{patch:int}/products")]
    [Route("api/v{version:apiVersion}/product")]
    [ApiController]
    [ApiVersion("2.0")]
    public class ProductAPIController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private Response _response;

        public ProductAPIController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _response = new Response();
        }

        [HttpGet("GetAllProduct")]
        public async Task<ActionResult<Response>> GetAllProduct()
        {
            try
            {
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _response.StatusCode = HttpStatusCode.NotFound;
            }
            return _response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> Get(string id)
        {
            try
            {

                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _response.StatusCode = HttpStatusCode.NotFound;
            }
            return _response;
        }

        [HttpGet]
        // [ResponseCache(CacheProfileName = "Default10")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response>> Get(
                                                    [FromQuery] int pageSize = 6,
                                                    [FromQuery] int currentPage = 1,
                                                    [FromQuery] string? search = ""
                                                    )
        {
            try
            {


                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Response>> Create(Product product)
        {
            try
            {

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
        public async Task<ActionResult<Response>> Update(Product product)
        {
            try
            {

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
        public async Task<ActionResult<Response>> Remove(string id)
        {
            try
            {

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
