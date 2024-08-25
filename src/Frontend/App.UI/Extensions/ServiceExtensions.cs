using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using App.Data.Models;
using App.Data;
using App.UI.Services.mail;
using App.UI.Controllers;
using App.Data.Repository.IRepository;
using App.Data.Repository;

namespace App.UI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddControllersWithViews();
            services.AddRazorPages();

            // Configure Authentication and Google Authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/Account/Login";
                    })
                    .AddGoogle(options =>
                    {
                        var googleConfig = configuration.GetSection("Authentication:Google");
                        options.ClientId = googleConfig["ClientId"];
                        options.ClientSecret = googleConfig["ClientSecret"];
                        options.CallbackPath = "/LoginWithGoogle";
                    });

            services.AddAuthorization();

            // Configure DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Configure Identity
            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            // Configure IdentityOptions
            services.Configure<IdentityOptions>(options =>
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

            // Configure Session
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Configure Email service
            services.AddOptions();
            var mailSettings = configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailSettings);
            // Register Email
            services.AddSingleton<IEmailSender, SendMailService>();

            // Register ProductRepository
            services.AddScoped<IProductRepository, ProductRepository>();

            // Register UnitOfWork
            services.AddScoped<UnitOfWork>();

            // Register CartService and HttpContextAccessor
            services.AddScoped<CartService>();
            services.AddHttpContextAccessor();

            // Configure Cookie
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.OnAppendCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });

            return services;
        }

        private static void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                if (DisallowsSameSiteNone(userAgent))
                {
                    options.SameSite = SameSiteMode.Unspecified;
                }
            }
        }

        private static bool DisallowsSameSiteNone(string userAgent)
        {
            if (string.IsNullOrWhiteSpace(userAgent))
            {
                return false;
            }

            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
            {
                return true;
            }

            return false;
        }
    }
}