
using App.Frontend.Models;
using App.Frontend.Services.IServices;
using App.Frontend.Utility;

namespace App.Frontend.Services
{
    public class CartService : ICartService
    {
        public readonly IBaseService _baseService;

        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<Response> GetAsync(string userId, string token)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.ShoppingCartAPIBase + "/api/cart/Checkout/" + userId,
                Token = token
            });
        }

        public async Task<Response> RemoveAsync(int cartDetailsId, string token)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = cartDetailsId,
                Url = StaticDetail.ShoppingCartAPIBase + "/api/cart/Remove",
                Token = token
            });
        }

        public async Task<Response> AddAsync(Cart cart, string token)
        {

            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = cart,
                Url = StaticDetail.ShoppingCartAPIBase + "/api/cart/Add",
                Token = token
            });
        }
    }
}