using System.Net;
using App.Services.OrderAPI.Data;
using App.Services.OrderAPI.Models;
using App.Services.OrderAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace App.Services.OrderAPI.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/order")]
    public class ReadOrderAPIController : ControllerBase
    {
        protected Response _response;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<WriteOrderAPIController> _logger;


        public ReadOrderAPIController(
                                    ApplicationDbContext dbContext,
                                     ILogger<WriteOrderAPIController> logger
                                    )
        {
            _response = new();
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet("GetAllOrder")]
        [ResponseCache(CacheProfileName = "Default10")]
        public async Task<ActionResult<Response?>> GetAllOrder([FromQuery] string userId)
        {
            try
            {
                if (userId == null)
                {
                    return _response;
                }

                IEnumerable<OrderHeader> orders;
                if (User.IsInRole(StaticDetail.RoleAdmin))
                {
                    orders = await _dbContext.OrderHeaders.AsNoTracking()
                                                            .Include(u => u.OrderDetails)
                                                            .OrderByDescending(u => u.Id)
                                                            .ToListAsync();
                    // select
                }
                else
                {
                    orders = await _dbContext.OrderHeaders.AsNoTracking().Include(u => u.OrderDetails).Where(u => u.UserId == userId).OrderByDescending(u => u.Id).ToListAsync();
                }

                if (orders == null)
                {
                    _response.Message = "Internal Server Error";
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.InternalServerError;
                }
                _response.Result = orders;
            }
            catch (Exception ex)
            {
                _response.Message = "Internal Server Error";
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _logger.LogError(ex.Message);
            }
            return _response;
        }

        [HttpGet("{orderId}")]
        [ResponseCache(CacheProfileName = "Default10")]
        public async Task<ActionResult<Response?>> Get([FromQuery] string orderId)
        {
            if (orderId == null)
            {
                return _response;
            }

            try
            {
                var orderHead = await _dbContext.OrderHeaders.AsNoTracking().Include(u => u.OrderDetails).FirstOrDefaultAsync(u => u.Id == orderId);
                if (orderHead == null)
                {
                    _response.Message = "Internal Server Error";
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.InternalServerError;
                }
                _response.Result = orderHead;
            }
            catch (Exception ex)
            {
                _response.Message = "Internal Server Error";
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _logger.LogError(ex.Message);
            }
            return _response;
        }
    }
}