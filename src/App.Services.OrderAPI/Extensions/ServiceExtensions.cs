using App.Services.OrderAPI.Models;
using App.Services.OrderAPI.Services;
using App.Services.OrderAPI.Services.IServices;
using App.Services.OrderAPI.Utility;

namespace App.Services.OrderAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AppServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ApiAuthenticationHttpClientHandler>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<Response>();
            services.AddHttpContextAccessor();
            services.AddHttpClient("Product", u => u.BaseAddress = new Uri(configuration["ServiceUrls:ProductAPI"])).AddHttpMessageHandler<ApiAuthenticationHttpClientHandler>();

            return services;
        }
    }
}