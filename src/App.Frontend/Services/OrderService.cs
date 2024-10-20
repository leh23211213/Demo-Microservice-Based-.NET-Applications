using App.Frontend.Models;
using App.Frontend.Services.IServices;
using App.Frontend.Utility;

namespace App.Frontend.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;

        public OrderService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<Response?> GetAllOrder(string? userId)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.OrderAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/order/GetOrders?userId=" + userId,
            });
        }

        public async Task<Response?> Get(string orderId)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.OrderAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/order/Get/" + orderId
            });
        }

        public async Task<Response?> CreateOrder(Cart cart)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = cart,
                Url = StaticDetail.OrderAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/order/CreateOrder"
            });
        }

        public async Task<Response?> CreateStripeSession(StripeRequest stripeRequest)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = stripeRequest,
                Url = StaticDetail.OrderAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/order/CreateStripeSession"
            });
        }


        public async Task<Response?> UpdateOrderStatus(string orderId, string newStatus)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = newStatus,
                Url = StaticDetail.OrderAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/order/UpdateOrderStatus" + orderId
            });
        }

        public async Task<Response?> ValidateStripeSession(string orderHeaderId)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = orderHeaderId,
                Url = StaticDetail.OrderAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/order/ValidateStripeSession"
            });
        }
    }
}