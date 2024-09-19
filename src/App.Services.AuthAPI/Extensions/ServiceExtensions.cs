using App.Services.AuthAPI.Data;
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Services;
using App.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace App.Services.AuthAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AppServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            // *************

            // Configure Identity
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
                   .AddDefaultTokenProviders();
            services.AddScoped<IAuthAPIService, AuthAPIService>();
            // *************
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAuthorization();
            services.AddAuthentication();
            return services;
        }
    }
}