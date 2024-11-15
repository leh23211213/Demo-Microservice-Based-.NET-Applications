using App.Domain.Admin.Models;
using App.Domain.Admin.Services;
using App.Domain.Admin.Utility;

namespace App.Domain.Admin.Services
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
                Url = StaticDetail.OrderAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/order/GetAllOrder?userId=" + userId,
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