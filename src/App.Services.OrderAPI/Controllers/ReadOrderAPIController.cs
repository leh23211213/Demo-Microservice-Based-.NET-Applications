using System.Net;
using App.Services.OrderAPI.Data;
using App.Services.OrderAPI.Models;
using App.Services.OrderAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace App.Services.OrderAPI.Controllers
{
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/order")]
    public class ReadOrderAPIController : ControllerBase
    {
        protected Response _response;
        private readonly ApplicationDbContext _dbContext;

        public ReadOrderAPIController(
                                    ApplicationDbContext dbContext
                                    )
        {
            _response = new();
            _dbContext = dbContext;
        }

        [HttpGet("GetAllOrder")]
        public async Task<ActionResult<Response?>> GetAllOrder(string? userId = "")
        {
            try
            {
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
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                }
                _response.Result = orders;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
            }
            return _response;
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<Response?>> Get(string orderId)
        {
            try
            {
                var orderHead = await _dbContext.OrderHeaders.AsNoTracking().Include(u => u.OrderDetails).FirstOrDefaultAsync(u => u.Id == orderId);
                _response.Result = orderHead;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
            }
            return _response;
        }
    }
}