using App.Frontend.Models;
using App.Frontend.Services.IServices;
using App.Frontend.Utility;

namespace App.Frontend.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<Response> GetAsync(string? search, int currentPage)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.ProductAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/product?currentPage=" + currentPage,
            });
        }

        public async Task<Response> GetAsync(string id)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.ProductAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/product/" + id,
            });
        }

        public async Task<Response> CreateAsync(Product product, string token)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = product,
                Url = StaticDetail.ProductAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/product",
                Token = token
            });
        }

        public async Task<Response> UpdateAsync(Product product, string token)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.PUT,
                Data = product,
                Url = StaticDetail.ProductAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/product",
                Token = token
            });
        }

        public async Task<Response> Delete(string id, string token)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.DELETE,
                Url = StaticDetail.ProductAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/product/" + id,
                Token = token
            });
        }
    }
}