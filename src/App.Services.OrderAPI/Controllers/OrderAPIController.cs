using System.Net;
using App.Services.Bus;
using App.Services.OrderAPI.Data;
using App.Services.OrderAPI.Models;
using App.Services.OrderAPI.Models.Dtos;
using App.Services.OrderAPI.Services.IServices;
using App.Services.OrderAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Services.OrderAPI.Controllers
{
    [Route("api/v{version:apiVersion}/order")]
    [ApiController]
    [ApiVersion("1.0")]
    public class OrderAPIController : ControllerBase
    {
        protected Response _response;
        private readonly ApplicationDbContext _dbContext;
        private IProductService _productService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;

        public OrderAPIController(
                                    ApplicationDbContext dbContext,
                                    IProductService productService,
                                    IMessageBus messageBus,
                                    IConfiguration configuration
                                    )
        {
            _response = new();
            _dbContext = dbContext;
            _productService = productService;
            _messageBus = messageBus;
            _configuration = configuration;
        }

        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<Response?>> Get(string? userId = "")
        {
            try
            {
                IEnumerable<OrderHeader> orders;

                if (User.IsInRole(StaticDetail.RoleAdmin))
                {
                    orders = await _dbContext.OrderHeaders.Include(u => u.OrderDetails).OrderByDescending(u => u.Id).ToListAsync();
                }
                else
                {
                    orders = await _dbContext.OrderHeaders.Include(u => u.OrderDetails).Where(u => u.UserId == userId).OrderByDescending(u => u.Id).ToListAsync();
                }
                _response.Result = orders;
                _response.Message = "";
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
            }
            return _response;
        }

        // [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response?>> Get(int orderId)
        {
            try
            {
                var orderHead = await _dbContext.OrderHeaders.Include(u => u.OrderDetails).FirstOrDefaultAsync(u => u.Id == orderId);

                _response.Result = orderHead;
                _response.Message = "";
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
            }
            return _response;
        }



        // [Authorize]
        [HttpPost("Create")]
        public async Task<ActionResult<Response>> Create([FromBody] Cart cart)
        {
            try
            {


                _response.Result = "";
                _response.Message = "";
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
            }

            return _response;
        }

        // [Authorize]
        [HttpPost("CreateStripeSession")]
        public async Task<ActionResult<Response>> CreateStripeSession([FromBody]StripeRequest stripeRequest)
        {
            try
            {
                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = "https://example.com/success",
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
    {
        new Stripe.Checkout.SessionLineItemOptions
        {
            Price = "price_1MotwRLkdIwHu7ixYcPLm5uZ",
            Quantity = 2,
        },
    },
                    Mode = "payment",
                };
                var service = new Stripe.Checkout.SessionService();
                service.Create(options);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
            }

            return _response;
        }

        //[Authorize]
        [HttpPost("ValidateStripeSession")]
        public async Task<ActionResult<Response>> ValidateStripeSession([FromBody] Cart cart)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
            }

            return _response;
        }

        //[Authorize]
        [HttpPost("UpdateStatus/{orderId:int}")]
        public async Task<ActionResult<Response>> UpdateStatus([FromBody] Cart cart)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
            }

            return _response;
        }
    }
}