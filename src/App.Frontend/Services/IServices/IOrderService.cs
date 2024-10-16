using App.Frontend.Models;

namespace App.Frontend.Services.IServices
{
    public interface IOrderService
    {
        Task<Response> CreateOrder(Cart cart);
        Task<Response> CreateStripeSession(StripeRequest stripeRequest);
        Task<Response> ValidateStripeSession(int orderHeaderId);
        /// <summary>
        /// Get all order by userid
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        Task<Response> Get(string userId);
        /// <summary>
        /// Get order by orderid
        /// </summary>
        /// <param name="oderId"></param>
        /// <returns></returns>
        Task<Response> Get(int oderId);
        Task<Response> UpdateOrderStatus(int orderId, string newStatus);
    }
}