using App.Services.Bus;
<<<<<<< HEAD
using App.Services.AuthAPI.Data;
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Services;
using Microsoft.AspNetCore.Identity;
using App.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
=======
using App.Services.AuthAPI.Services;
using App.Services.AuthAPI.Services;
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
namespace App.Services.AuthAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AppServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
<<<<<<< HEAD
=======
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
            // Configure Identity
            services.AddScoped<ITokenProvider, TokenProvider>();
            services.AddScoped<IAuthAPIService, AuthAPIService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
<<<<<<< HEAD
            services.AddIdentity<ApplicationUser, IdentityRole>()
                                .AddEntityFrameworkStores<ApplicationDbContext>()
                                .AddDefaultTokenProviders();
            services.AddScoped<IMessageBus, MessageBus>();
            // *************
=======
            services.AddScoped<IMessageBus, MessageBus>();

>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
            return services;
        }
    }
}