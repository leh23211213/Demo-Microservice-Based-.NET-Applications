using App.Frontend.Models;
using App.Frontend.Utility;

namespace App.Frontend.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Get
        /// </summary>
        Task<Response?> Get();

        /// <summary>
        /// Get product by Id
        /// </summary>
        Task<Response?> Get(string id);

        /// <summary>
        ///  Pagination/$"?pageSize={pageSize}+search={search}+currentPage={currentPage} 
        /// </summary>
        Task<Response?> Get(int pageSize, int currentPage, string? search);

        Task<Response?> CreateAsync(Product product);
        Task<Response?> UpdateAsync(Product product);
        Task<Response?> DeleteAsync(string id);
    }

    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<Response?> Get()
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.ProductAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/product"
            }, withBearer: true);
        }

        public async Task<Response?> Get(string id)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.ProductAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/product/" + id,
            }, withBearer: true);
        }

        public async Task<Response?> Get(int pageSize, int currentPage, string? search)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.ProductAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/product/Pagination/" + $"?pageSize={pageSize}&search={search}&currentPage={currentPage}",
            }, withBearer: true);
        }

        public async Task<Response?> CreateAsync(Product product)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = product,
                Url = StaticDetail.ProductAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/product/Create",
                ContentType = StaticDetail.ContentType.MultipartFormData
            });
        }

        public async Task<Response?> UpdateAsync(Product product)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.PUT,
                Data = product,
                Url = StaticDetail.ProductAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/product/Update",
                ContentType = StaticDetail.ContentType.MultipartFormData
            });
        }

        public async Task<Response?> DeleteAsync(string id)
        {
            return await _baseService.SendAsync(new Request
            {
                ApiType = StaticDetail.ApiType.DELETE,
                Url = StaticDetail.ProductAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/product/Delete/" + id
            });
        }
    }
}