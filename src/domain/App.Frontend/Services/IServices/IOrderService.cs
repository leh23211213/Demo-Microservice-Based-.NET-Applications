using App.Frontend.Models;

namespace App.Frontend.Services.IServices
{
    public interface IOrderService
    {
        Task<Response> CreateOrder(Cart cart);
        Task<Response> CreateStripeSession(StripeRequest stripeRequest);
        Task<Response> ValidateStripeSession(string orderHeaderId);
        /// <summary>
        /// Get all order by userid
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        Task<Response> GetAllOrder(string? userId);
        /// <summary>
        /// Get order by orderid
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<Response> Get(string orderId);
        Task<Response> UpdateOrderStatus(string orderId, string newStatus);
    }
}