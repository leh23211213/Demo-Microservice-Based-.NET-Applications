using App.Services.Bus;
using App.Services.OrderAPI.Models;
using App.Services.OrderAPI.Services;
using App.Services.OrderAPI.Services.IServices;
using App.Services.OrderAPI.Utility;
using AutoMapper;

namespace App.Services.OrderAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AppServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ApiAuthenticationHttpClientHandler>();

            IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHttpContextAccessor();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IMessageBus, MessageBus>();
            services.AddScoped<Response>();
            services.AddHttpClient("Product", u => u.BaseAddress =
                                                    new Uri(configuration["ServiceUrls:ProductAPI"]))
                                                    .AddHttpMessageHandler<ApiAuthenticationHttpClientHandler>();
            return services;
        }
    }
}