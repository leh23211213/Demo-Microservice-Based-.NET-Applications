using App.Services.Bus;
using App.Services.AuthAPI.Services;
using Duende.IdentityServer.Services;
namespace App.Services.AuthAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AppServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // Configure Identity
            services.AddScoped<ITokenProvider, TokenProvider>();
            services.AddScoped<IAuthAPIService, AuthAPIService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IMessageBus, MessageBus>();
            services.AddAntiforgery();
            services.AddRazorPages();
            services.AddScoped<IProfileService, ProfileService>();

            return services;
        }
    }
}