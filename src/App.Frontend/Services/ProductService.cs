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

        public async Task<Response?> GetAllProduct()
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.ProductAPIBase + "/api/product/GetAllProduct"
            }, withBearer: false);
        }

        public async Task<Response?> Get(string id)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.ProductAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/product/" + id,
            }, withBearer: false);
        }

        public async Task<Response?> Get(string? search, int currentPage)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.ProductAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/product/" + $"?search={search}&currentPage={currentPage}",
            }, withBearer: false);
        }


        public async Task<Response?> CreateAsync(Product product)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = product,
                Url = StaticDetail.ProductAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/product",
            });
        }

        public async Task<Response?> UpdateAsync(Product product)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.PUT,
                Data = product,
                Url = StaticDetail.ProductAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/product",
            });
        }

        public async Task<Response?> DeleteAsync(string id)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.DELETE,
                Url = StaticDetail.ProductAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/product/" + id,
            });
        }
    }
}