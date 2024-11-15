
using App.Frontend.Models;
using App.Frontend.Utility;

namespace App.Frontend.Services
{
    public interface ICartService
    {
        /// <summary>
        /// StaticDetail.ShoppingCartAPIBase + "/api/version/cart/Get" + userId,
        /// </summary>
        Task<Response> GetAsync(string userId);
        /// <summary>
        /// StaticDetail.ShoppingCartAPIBase + "/api/version/cart/Delete" + cartDetailsId,
        /// </summary>
        Task<Response> DeleteAsync(string cartDetailsId);

        /// <summary>
        /// StaticDetail.ShoppingCartAPIBase + "/api/version/cart/Add",
        /// </summary>
        Task<Response> AddAsync(Cart cart);
    }

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