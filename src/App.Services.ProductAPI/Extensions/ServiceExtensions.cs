using System.Text;
using App.Services.ProductAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace App.Services.ProductAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AppServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container. 
            services.AddResponseCaching();
            // services.AddControllers(option =>
            //  {
            //      option.CacheProfiles.Add("Default10",
            //         new CacheProfile()
            //         {
            //             Duration = 10
            //         });
            //      //option.ReturnHttpNotAcceptable=true;
            //  }).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
            services.AddControllers();
            return services;
        }
    }
}