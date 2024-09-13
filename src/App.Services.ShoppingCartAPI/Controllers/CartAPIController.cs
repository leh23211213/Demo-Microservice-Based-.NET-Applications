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

        [HttpGet("checkout/cart/{userId}")]
        public async Task<Response> CheckOut(string userId)
        {
            try
            {
                var cartHeader = await _dbContext.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId);
                if (cartHeader == null)
                {
                    _dbContext.CartHeaders.Add(cartHeader);
                    await _dbContext.SaveChangesAsync();
                }
                IEnumerable<Product> products = await _productService.GetAsync();
                _response.Result = products;
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