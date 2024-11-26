
using App.Services.OrderAPI.Models;
using App.Services.OrderAPI.Services;

namespace App.Services.OrderAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AppServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<Utility.ApiAuthenticationHttpClientHandler>();

            AutoMapper.IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddHttpContextAccessor();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<Bus.IMessageBus, Bus.MessageBus>();
            services.AddTransient<Response>();

            services.AddHttpClient("Product", u => u.BaseAddress =
                                                    new Uri(configuration["ServiceUrls:ProductAPI"]))
                                                    .AddHttpMessageHandler<Utility.ApiAuthenticationHttpClientHandler>();

            services.AddResponseCaching();

            services.AddControllers(options =>
            {
                options.CacheProfiles.Add("Default10", new Microsoft.AspNetCore.Mvc.CacheProfile
                {
                    Duration = 10
                });
            })
            .AddNewtonsoftJson(options =>
            {
                // If you need to configure Newtonsoft.Json settings, do it here
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            })
            .AddXmlDataContractSerializerFormatters();
            /*
            e. Use Compression
            Enable GZIP compression in your API responses to reduce payload size and improve API response time.
            */
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
                options.EnableForHttps = true;
            });


            return services;
        }
    }
}