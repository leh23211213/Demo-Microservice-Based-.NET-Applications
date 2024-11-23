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
            services.AddScoped<IRegisterAPIService, RegisterAPIService>();
            services.AddScoped<ILoginAPIService, LoginAPIService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IMessageBus, MessageBus>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddAntiforgery();
            //services.AddRazorPages();

            return services;
        }
    }
}