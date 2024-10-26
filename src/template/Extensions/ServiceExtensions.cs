namespace template.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AppServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container. 
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