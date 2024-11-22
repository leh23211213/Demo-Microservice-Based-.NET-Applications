using App.Services.ShoppingCartAPI.Models;
using Newtonsoft.Json;

namespace App.Services.ShoppingCartAPI.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Get all product from product api
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetAsync();
    }
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<Product>> GetAsync()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/v1/product");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<Response>(apiContent);
            if (resp.Result != null && resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<Product>>(Convert.ToString(resp.Result));
            }
            return new List<Product>();
        }
    }
}