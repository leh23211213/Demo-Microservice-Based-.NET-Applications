using System.Net;
using App.Services.ShoppingCartAPI.Data;
using App.Services.ShoppingCartAPI.Models;
using App.Services.ShoppingCartAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Services.ShoppingCartAPI.Controllers
{
    [Route("api/v{version:apiVersion}/cart")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CartAPIController : Controller
    {
        private Response _response;
        private IProductService _productService;
        private readonly ApplicationDbContext _dbContext;

        public CartAPIController(Response response,
                                 IProductService productService,
                                ApplicationDbContext dbContext)
        {
            _response = response;
            _productService = productService;
            _dbContext = dbContext;
        }

        [HttpGet("Get")]
        public async Task<Response> Checkout(string userId)
        {
            try
            {
                Cart cart = new()
                {
                    CartHeader = await _dbContext.CartHeaders.AsNoTracking().FirstAsync(u => u.UserId == userId)
                };
                cart.CartDetails = _dbContext.CartDetails.Where(u => u.CartHeaderId == cart.CartHeader.Id);

                IEnumerable<Product> products = await _productService.GetAsync();

                foreach (var item in cart.CartDetails)
                {
                    item.Product = products.FirstOrDefault(u => u.Id == item.ProductId);
                    cart.CartHeader.Total += (item.Count * item.Product.Price);
                }

                // if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                // {

                // }

                _response.Result = cart;
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

        [HttpPost("Add")]
        public async Task<Response> Add(Cart cart)
        {
            try
            {
                var cartHeaderFromDb = await _dbContext.CartHeaders.FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.UserId);

                if (cartHeaderFromDb == null)
                {
                    //create header and details
                    var cartHeader = cart.CartHeader;
                    _dbContext.Add(cartHeader);
                    await _dbContext.SaveChangesAsync();

                    cart.CartDetails.First().CartHeaderId = cartHeader.Id;
                    _dbContext.Add(cart.CartDetails.First());
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    //check if details has same product
                    var cartDetailsFromDb = await _dbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cart.CartDetails.First().ProductId &&
                        u.CartHeaderId == cartHeaderFromDb.Id
                    );

                    if (cartDetailsFromDb == null)
                    {
                        //create cartdetails
                        cart.CartDetails.First().CartHeaderId = cartHeaderFromDb.Id;
                        _dbContext.CartDetails.Add(cart.CartDetails.First());
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in cart details
                        _response.Message = "Product are ready exists in your cart!";
                    }
                }

                _response.IsSuccess = true;
                _response.Result = cart;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                _response.StatusCode = HttpStatusCode.BadRequest;
            }
            return _response;
        }

        [HttpDelete("RemoveItem")]
        public async Task<Response> Remove([FromBody] string cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _dbContext.CartDetails.First(u => u.Id == cartDetailsId);
                int countCartItem = _dbContext.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();

                _dbContext.CartDetails.Remove(cartDetails);


                await _dbContext.SaveChangesAsync();
                _response.Result = true;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _response.StatusCode = HttpStatusCode.BadRequest;
            }
            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<Response> ApplyCoupon([FromBody] Cart cart)
        {
            try
            {

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
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