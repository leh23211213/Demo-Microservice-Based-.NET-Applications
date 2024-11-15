<<<<<<< HEAD
using App.Domain.Admin.Extensions;
using App.Domain.Admin.Services;
using App.Domain.Admin.Services.IServices;
using App.Domain.Admin.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
=======
<<<<<<<< HEAD:src/domain/App.Frontend/Program.cs
using App.Frontend.Utility;
using App.Frontend.Services;
using App.Frontend.Extensions;
using App.Frontend.Services.IServices;
using Microsoft.AspNetCore.Identity;
========
using App.Domain.Admin.Utility;
using App.Domain.Admin.Services;
using App.Domain.Admin.Extensions;
using App.Domain.Admin.Services.IServices;
using Microsoft.AspNetCore.Authentication;
>>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/domain/App.Domain.Admin/Program.cs
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews(u => u.Filters.Add(new AuthExceptionRedirection()));
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IApiMessageRequestBuilder, ApiMessageRequestBuilder>();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<ICartService, CartService>();
builder.Services.AddHttpClient<IOrderService, OrderService>();

StaticDetail.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
StaticDetail.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"];
StaticDetail.ShoppingCartAPIBase = builder.Configuration["ServiceUrls:ShoppingCartAPI"];
StaticDetail.OrderAPIBase = builder.Configuration["ServiceUrls:OrderAPI"];

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
<<<<<<< HEAD
=======
<<<<<<<< HEAD:src/domain/App.Frontend/Program.cs
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
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
<<<<<<< HEAD
=======
========
>>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/domain/App.Domain.Admin/Program.cs
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0

/*
Cookie Authentication: Used to manage user authentication. 
It stores the user's authentication ticket (e.g., login status) in a cookie.
 This allows the server to know if a user is authenticated on subsequent requests.
*/
<<<<<<< HEAD
=======
<<<<<<<< HEAD:src/domain/App.Frontend/Program.cs
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // the expiration time for the authentication cookie
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.SlidingExpiration = true; // sẽ giúp gia hạn thời gian sống của cookie mỗi khi có hoạt động. Tuy nhiên, nếu không có hoạt động hoặc phiên đã hết hạn, người dùng sẽ bị đăng xuất và chuyển hướng về trang login.
    });
<<<<<<< HEAD

=======
========
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "OpenIdConnect";
})
.AddJwtBearer()
.AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.LoginPath = "/Authentication/Login";
    options.AccessDeniedPath = "/Authentication/AccessDenied";
})
.AddOpenIdConnect("OpenIdConnect", options =>
{
    options.Authority = builder.Configuration["ServiceUrls:AuthAPI"];
    //Claims wil have every detail information
    options.GetClaimsFromUserInfoEndpoint = true;
    //
    options.ClientId = "admin_scope";
    options.ClientSecret = StaticDetail.secret;
    options.ResponseType = "code";
    //
    options.TokenValidationParameters.NameClaimType = "name";
    options.TokenValidationParameters.RoleClaimType = "role";
    //
    options.Scope.Add("admin_scope");
    options.SaveTokens = true;
>>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/domain/App.Domain.Admin/Program.cs

    options.ClaimActions.MapJsonKey("role", "role");
    options.Events = new OpenIdConnectEvents
    {
        OnRemoteFailure = context =>
        {
            context.Response.Redirect("/");
            context.HandleResponse();
            return Task.FromResult(0);
        }
    };
});
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
/*
Session: Used to store user-specific data on the server (such as shopping cart information, preferences, etc.).
 The session can be used to store non-authentication-related information temporarily and is tied to the user’s session ID.
 This handles storing session-specific data (like cart items or temporary form data) and sets an idle
*/
<<<<<<< HEAD
=======
<<<<<<<< HEAD:src/domain/App.Frontend/Program.cs
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(7);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;  // Make the session cookie essential

});

<<<<<<< HEAD

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

=======
========
>>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/domain/App.Domain.Admin/Program.cs

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

<<<<<<<< HEAD:src/domain/App.Frontend/Program.cs
app.UseSession();
app.UseHttpsRedirection();
========
>>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/domain/App.Domain.Admin/Program.cs
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Default route
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=CoverPage}/{id?}");

app.Run();
