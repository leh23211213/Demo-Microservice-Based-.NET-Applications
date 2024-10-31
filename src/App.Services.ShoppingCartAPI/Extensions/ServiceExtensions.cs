namespace App.Services.ShoppingCartAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AppServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<Utility.ApiAuthenticationHttpClientHandler>();
            services.AddScoped<Services.IServices.IProductService, Services.ProductService>();
            services.AddScoped<Models.Response>();
            services.AddHttpContextAccessor();
            services.AddHttpClient("Product", u => u.BaseAddress = new Uri(configuration["ServiceUrls:ProductAPI"])).AddHttpMessageHandler<Utility.ApiAuthenticationHttpClientHandler>();

            services.AddControllers()
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