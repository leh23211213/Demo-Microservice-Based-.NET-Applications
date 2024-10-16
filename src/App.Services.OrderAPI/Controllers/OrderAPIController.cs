using System.Net;
using App.Services.Bus;
using App.Services.OrderAPI.Data;
using App.Services.OrderAPI.Models;
using App.Services.OrderAPI.Services.IServices;
using App.Services.OrderAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace App.Services.OrderAPI.Controllers
{
    [Route("api/v{version:apiVersion}/order")]
    [ApiController]
    [ApiVersion("1.0")]
    public class OrderAPIController : ControllerBase
    {
        private readonly ILogger<OrderAPIController> _logger;
        protected Response _response;
        private readonly ApplicationDbContext _dbContext;
        private IProductService _productService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;

        public OrderAPIController(
                                    ApplicationDbContext dbContext,
                                    IProductService productService,
                                    IMessageBus messageBus,
                                    IConfiguration configuration,
                                    ILogger<OrderAPIController> logger
                                    )
        {
            _response = new();
            _dbContext = dbContext;
            _productService = productService;
            _messageBus = messageBus;
            _configuration = configuration;
            _logger = logger;
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
        public async Task<ActionResult<Response>> CreateStripeSession([FromBody] Models.StripeRequest stripeRequest)
        {
            try
            {
                var options = new SessionCreateOptions
                {
                    SuccessUrl = stripeRequest.ApprovedUrl,
                    CancelUrl = stripeRequest.CancelUrl,
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };

                var Discount = new List<SessionDiscountOptions>(){
                    new SessionDiscountOptions(){
                        Coupon = stripeRequest.OrderHeader.CouponCode,
                    }
                };

                foreach (var item in stripeRequest.OrderHeader.OrderDetails)
                {
                    var sessionLineItemOptions = new SessionLineItemOptions()
                    {
                        PriceData = new SessionLineItemPriceDataOptions()
                        {
                            UnitAmount = (long)(item.Price * 1000),
                            Currency = "VND",
                            ProductData = new SessionLineItemPriceDataProductDataOptions()
                            {
                                Name = item.Product.Name
                            },
                        },
                        Quantity = item.Count,
                    };

                    options.LineItems.Add(sessionLineItemOptions);
                }

                if (stripeRequest.OrderHeader.Discount > 0)
                {
                    options.Discounts = Discount;
                }

                var service = new SessionService();
                Session session = service.Create(options);
                stripeRequest.StripeSessionUrl = session.Url;
                OrderHeader orderHeader = _dbContext.OrderHeaders.First(u => u.Id == stripeRequest.OrderHeader.Id);
                orderHeader.StripeSessionId = session.Id;
                _dbContext.SaveChanges();

                _response.Result = stripeRequest;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;

                _logger.LogError(ex.Message);
            }

            return _response;
        }

        //[Authorize]
        [HttpPost("ValidateStripeSession")]
        public async Task<ActionResult<Response>> ValidateStripeSession([FromBody] int orderHeaderId)
        {
            try
            {
                OrderHeader orderHeader = _dbContext.OrderHeaders.First(u => u.Id == orderHeaderId);

                var service = new SessionService();
                Session session = service.Get(orderHeader.StripeSessionId);

                var paymentIntentService = new PaymentIntentService();
                PaymentIntent paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

                if (paymentIntent.Status == "succeeded")
                {
                    orderHeader.PaymentIntentId = paymentIntent.Id;
                    orderHeader.Status = paymentIntent.Status;
                    _dbContext.SaveChanges();

                    _response.Result = orderHeader;
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                }

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