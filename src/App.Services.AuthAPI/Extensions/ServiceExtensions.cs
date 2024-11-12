using App.Services.Bus;
using App.Services.AuthAPI.Data;
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Services;
using Microsoft.AspNetCore.Identity;
using App.Services.AuthAPI.Services.IServices;

namespace App.Services.AuthAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AppServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure Identity
            services.AddScoped<ITokenProvider, TokenProvider>();
            services.AddScoped<IAuthAPIService, AuthAPIService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                                .AddEntityFrameworkStores<ApplicationDbContext>()
                                .AddDefaultTokenProviders();
            services.AddScoped<IMessageBus, MessageBus>();
            // *************
            return services;
        }
    }
}