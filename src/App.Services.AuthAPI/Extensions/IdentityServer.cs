// Lấy chứng chỉ từ Windows Certificate Store hoặc file PFX
// var certificate = GetSigningCertificate(configuration);
// use the dotnet identity 
/*
    typeof(Config):
    Lấy kiểu (type) của lớp Config.
    Đây là lớp bạn định nghĩa trong ứng dụng (có thể là cấu hình cho IdentityServer hoặc các dịch vụ khác).
    
    .Assembly:
    Trả về thông tin assembly mà lớp Config thuộc về.
    Assembly là một khối triển khai cơ bản trong .NET, chứa các metadata, mã thực thi (IL), và các resource.

    .GetName():
    Lấy tên của assembly, bao gồm các thông tin như tên assembly, version, và các thông tin khác.

    .Name:
    Lấy tên assembly dưới dạng chuỗi (chỉ phần tên, không bao gồm phần mở rộng .dll).

    Kết quả:
    Biến migrationsAssembly sẽ chứa tên của assembly mà lớp Config thuộc về, ví dụ: "YourAppNamespace".
    
dotnet ef migrations add InitialIdentityServerConfiguration -c ConfigurationDbContext
dotnet ef migrations add InitialIdentityServerOperational -c PersistedGrantDbContext
dotnet ef migrations add InitialIdentityServerMigration -c ApplicationDbContext
dotnet ef database update -c

*/
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Data;
using System.Security.Cryptography.X509Certificates;
using App.Services.AuthAPI.Utility;
using Microsoft.EntityFrameworkCore;

namespace App.Services.AuthAPI.Extensions
{
    public static class IdentityServer
    {
        public static WebApplicationBuilder AddIdentityServer7(this WebApplicationBuilder builder, IConfiguration configuration)
        {

            // var connectionString = configuration.GetConnectionString("DefaultSQLConnection");
            // var migrationsAssembly = typeof(StaticDetail).Assembly.GetName().Name;

            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
            });

            // builder.Services.AddDbContext<ApplicationDbContext>(options =>
            // {
            //     options.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));
            // });

            // builder.Services.AddIdentityServer(options =>
            // {
            //     options.Events.RaiseErrorEvents = true;
            //     options.Events.RaiseInformationEvents = true;
            //     options.Events.RaiseFailureEvents = true;
            //     options.Events.RaiseSuccessEvents = true;
            //     options.EmitStaticAudienceClaim = true;
            //     // Specify custom user interaction URLs
            //     // options.UserInteraction.LoginUrl = "/user/login";
            //     // options.UserInteraction.LogoutUrl = "/user/logout";
            // })
            // .AddConfigurationStore(options =>
            // {
            //     options.ConfigureDbContext = db =>
            //         db.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
            // })
            // .AddOperationalStore(options =>
            // {
            //     options.ConfigureDbContext = db =>
            //         db.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
            // })
            // .AddInMemoryIdentityResources(StaticDetail.IdentityResources)
            // .AddInMemoryApiScopes(StaticDetail.ApiScopes)
            // .AddInMemoryClients(StaticDetail.Clients)
            // .AddInMemoryApiResources(StaticDetail.ApiResources)
            // .AddAspNetIdentity<ApplicationUser>() // use the dotnet identity 
            // .AddProfileService<ProfileService>()
            // .AddDeveloperSigningCredential(); // For Development
            // //.AddSigningCredential(certificate)


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
