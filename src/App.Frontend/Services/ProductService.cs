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

        public async Task<Response> GetAsync(int currentPage, string token)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.ProductAPIBase + "/api/product/page/" + currentPage,
                Token = token
            });
        }

        public async Task<Response> GetAsync(string id, string token)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.ProductAPIBase + "/api/product/" + id,
                Token = token
            });
        }

        public async Task<Response> CreateAsync(Product product, string token)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = product,
                Url = StaticDetail.ProductAPIBase + "/api/product",
                Token = token
            });
        }

        public async Task<Response> UpdateAsync(Product product, string token)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.PUT,
                Data = product,
                Url = StaticDetail.ProductAPIBase + "/api/product",
                Token = token
            });
        }

        public async Task<Response> Delete(string id, string token)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.DELETE,
                Url = StaticDetail.ProductAPIBase + "/api/product/" + id,
                Token = token
            });
        }
    }
}