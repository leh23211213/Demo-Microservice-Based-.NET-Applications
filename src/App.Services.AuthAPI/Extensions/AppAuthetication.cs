using Microsoft.AspNetCore.Identity;

namespace App.Services.AuthAPI.Extensions
{
    public static class AppAuthetication
    {
        public static WebApplicationBuilder AddAppValidate(this WebApplicationBuilder builder)
        {
            // Configure IdentityOptions
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                // SignIn settings
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            builder.Services.AddAuthorization();
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<SecurityHeadersAttribute>();
            });
            // .AddJsonOptions(options =>
            // {
            //     options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            // });

            return builder;
        }
    }
}
