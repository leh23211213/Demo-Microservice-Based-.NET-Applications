using Stripe;
using System.Net;
using AutoMapper;
using Stripe.Checkout;
using Microsoft.AspNetCore.Mvc;
using App.Services.OrderAPI.Data;
using App.Services.OrderAPI.Models;
using App.Services.OrderAPI.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace App.Services.OrderAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "ADMIN")]
    [Route("api/v{version:apiVersion}/order")]
    public class WriteOrderAPIController : ControllerBase
    {
        private IMapper _mapper;
        protected Response _response;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<WriteOrderAPIController> _logger;

        public WriteOrderAPIController(
                                    IMapper mapper,
                                    ApplicationDbContext dbContext,
                                    ILogger<WriteOrderAPIController> logger
                                    )
        {
            _response = new();
            _mapper = mapper;
            _logger = logger;
            _dbContext = dbContext;
        }


        [HttpPost("CreateOrder")]
        public async Task<ActionResult<Response>> CreateOrder([FromBody] Cart cart)
        {
            if (cart == null) return _response;

            try
            {
                OrderHeader orderHeader = _mapper.Map<OrderHeader>(cart.CartHeader);
                orderHeader.Status = StaticDetail.Status_Pending;
                orderHeader.OrderTotal = Math.Round(orderHeader.OrderTotal ?? 0, 2);
                orderHeader.OrderDetails = _mapper.Map<IEnumerable<OrderDetails>>(cart.CartDetails);
                orderHeader.OrderTime = DateTime.Now;
                OrderHeader orderCreated = _dbContext.OrderHeaders.Add(orderHeader).Entity;
                await _dbContext.SaveChangesAsync();

                orderHeader.Id = orderCreated.Id;
                _response.Result = orderHeader;
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

        [HttpPost("CreateStripeSession")]
        public async Task<ActionResult<Response>> CreateStripeSession([FromBody] Models.StripeRequest stripeRequest)
        {
            if (stripeRequest == null) return _response;

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

        [HttpPost("ValidateStripeSession")]
        public async Task<ActionResult<Response>> ValidateStripeSession([FromBody] string orderHeaderId)
        {
            if (orderHeaderId == null) return _response;

            try
            {
                OrderHeader orderHeader = await _dbContext.OrderHeaders.FirstOrDefaultAsync(u => u.Id == orderHeaderId);
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
                }
            }
            catch (Exception ex)
            {
                _response.Message = "Internal Server Error";
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return _response;
        }

        [HttpPost("UpdateStatus")]
        public async Task<ActionResult<Response>> UpdateStatus([FromBody] Cart cart)
        {
            try
            {

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