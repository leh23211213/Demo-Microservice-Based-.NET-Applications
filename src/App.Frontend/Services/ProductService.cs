using App.Frontend.Models;
using App.Frontend.Services.IServices;
using App.Frontend.Utility;
using Microsoft.AspNetCore.Mvc;

namespace App.Frontend.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<Response> GetAsync(int currentPage)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.ProductAPIBase + "/api/product/page/" + currentPage
            });
        }

        public async Task<Response> GetAsync(string id)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<Response> CreateAsync(Product product)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = product,
                Url = StaticDetail.ProductAPIBase + "/api/product"
            });
        }

        public async Task<Response> UpdateAsync(Product product)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.PUT,
                Data = product,
                Url = StaticDetail.ProductAPIBase + "/api/product"
            });
        }

        public async Task<Response> Delete(string id)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.DELETE,
                Url = StaticDetail.ProductAPIBase + "/api/product/" + id
            });
        }
    }
}