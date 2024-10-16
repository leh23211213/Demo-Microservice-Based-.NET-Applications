using App.Frontend.Models;
using App.Frontend.Services.IServices;

namespace App.Frontend.Services
{
    public class OrderService : IOrderService
    {
        public async Task<Response> CreateOrder(Cart cart)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> CreateStripeSession(StripeRequest stripeRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> Get(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> Get(int oderId)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> UpdateOrderStatus(int orderId, string newStatus)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> ValidateStripeSession(int orderHeaderId)
        {
            throw new NotImplementedException();
        }
    }
}