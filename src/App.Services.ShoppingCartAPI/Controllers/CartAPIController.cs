using System.IdentityModel.Tokens.Jwt;
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
                                ApplicationDbContext _dbContext)
        {
            _response = response;
            _productService = productService;
            _dbContext = _dbContext;
        }

        [HttpGet("checkout/{userId}")]
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

                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {

                }

                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost("Add")]
        public async Task<Response> Add(Cart cart)
        {
            try
            {
                var cartHeaderFromDb = await _dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.Id);
                if (cartHeaderFromDb == null)
                {
                    CartHeader cartHeader = cartHeaderFromDb;
                    _dbContext.Add(cartHeader);
                    await _dbContext.SaveChangesAsync();

                    cart.CartDetails.First().CartHeaderId = cartHeader.Id;
                    _dbContext.Add(cart.CartDetails.First());
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    var cartDetailsFromDb = await _dbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cart.CartDetails.First().ProductId &&
                        u.CartHeaderId == cartHeaderFromDb.Id
                    );

                    if (cartDetailsFromDb == null)
                    {
                        cart.CartDetails.First().CartHeaderId = cartHeaderFromDb.Id;
                        _dbContext.CartDetails.Add(cart.CartDetails.First());
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        cart.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cart.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cart.CartDetails.First().Id = cartDetailsFromDb.Id;
                        _dbContext.CartDetails.Update(cart.CartDetails.First());
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        public async Task<Response> Remove([FromBody] string cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _dbContext.CartDetails.First(u => u.Id == cartDetailsId);
                
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