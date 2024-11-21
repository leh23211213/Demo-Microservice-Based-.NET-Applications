
using App.Domain.Admin.Models;
using App.Domain.Admin.Services.IServices;
using App.Domain.Admin.Utility;

namespace App.Domain.Admin.Services
{
    public class CartService : ICartService
    {
        public readonly IBaseService _baseService;

        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<Response?> GetAsync(string userId)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.ShoppingCartAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/cart/Checkout/" + userId,
            });
        }

        public async Task<Response?> DeleteAsync(string cartDetailsId)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.DELETE,
                Data = cartDetailsId,
                Url = StaticDetail.ShoppingCartAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/cart/Delete",
            });
        }

        public async Task<Response?> AddAsync(Cart cart)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = cart,
                Url = StaticDetail.ShoppingCartAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/cart/Add",
            });
        }
    }
}