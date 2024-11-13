using Microsoft.AspNetCore.Identity;
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Data;
using System.Security.Cryptography.X509Certificates;
using Duende.IdentityServer.Services;
using App.Services.AuthAPI.Utility;
namespace App.Services.AuthAPI.Extensions
{
    public static class IdentityServer
    {
        public static WebApplicationBuilder AddIdentityServer7(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            // Lấy chứng chỉ từ Windows Certificate Store hoặc file PFX
            // var certificate = GetSigningCertificate(configuration);
            // use the dotnet identity 
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
                
            builder.Services.AddRazorPages();

            builder.Services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
                // Specify custom user interaction URLs
                // options.UserInteraction.LoginUrl = "/user/login";
                // options.UserInteraction.LogoutUrl = "/user/logout";
            })
            .AddInMemoryIdentityResources(StaticDetail.IdentityResources)
            .AddInMemoryApiScopes(StaticDetail.ApiScopes)
            .AddInMemoryClients(StaticDetail.Clients)
            .AddAspNetIdentity<ApplicationUser>() // use the dotnet identity 
            .AddDeveloperSigningCredential() // For Development
            .AddProfileService<ProfileService>();
            //.AddSigningCredential(certificate)

            builder.Services.AddScoped<IProfileService, ProfileService>();

            return builder;
        }

        private static X509Certificate2 GetSigningCertificate(IConfiguration configuration)
        {
            var certificatePath = configuration["Certificate:Path"];
            var certificatePassword = configuration["Certificate:Password"];

            // Sử dụng chứng chỉ từ file PFX nếu có, nếu không thì tìm trong Windows Certificate Store
            if (!string.IsNullOrEmpty(certificatePath) && !string.IsNullOrEmpty(certificatePassword))
            {
                return new X509Certificate2(certificatePath, certificatePassword);
            }
            else
            {
                using var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                var certs = store.Certificates.Find(X509FindType.FindByThumbprint, configuration["Certificate:Thumbprint"], validOnly: false);
                if (certs.Count > 0)
                {
                    return certs[0];
                }
                throw new Exception("Signing certificate not found");
            }
        }
    }
}
