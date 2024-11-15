using App.Frontend.Models;

namespace App.Frontend.Services.IServices
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
}