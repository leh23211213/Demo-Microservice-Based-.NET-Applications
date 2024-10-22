using Microsoft.AspNetCore.Mvc;
namespace App.Services.ProductAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AppServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container. 
            services.AddResponseCaching();
            services.AddControllers(options =>
            {
                options.CacheProfiles.Add("Default10", new CacheProfile
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
            return services;
        }
    }
}