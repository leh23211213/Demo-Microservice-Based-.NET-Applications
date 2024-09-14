using App.Services.ShoppingCartAPI.Data;
using App.Services.ShoppingCartAPI.Models;
using App.Services.ShoppingCartAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
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

        [HttpGet("cart/get/{userId}")]
        public async Task<Response> CheckOut(string userId)
        {
            var cartHeader = await _dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
            try
            {
                if (cartHeader == null)
                {
                    cartHeader = new CartHeader()
                    {
                        UserId = userId,
                    };
                    _dbContext.CartHeaders.Add(cartHeader);
                    await _dbContext.SaveChangesAsync();
                }
                _response.Result = cartHeader;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        public async Task<Response> Add(string userId, string productId)
        {
            var cartHeader = await _dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
            try
            {
                if (cartHeader == null)
                {
                    cartHeader = new CartHeader()
                    {
                        UserId = userId,
                    };
                    _dbContext.CartHeaders.Add(cartHeader);
                    await _dbContext.SaveChangesAsync();


                }
                else
                {

                }
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