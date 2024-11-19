using App.Frontend.Utility;
using App.Frontend.Services;
using App.Frontend.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
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
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews(u => u.Filters.Add(new AuthExceptionRedirection()));

/*
Cookie Authentication: Used to manage user authentication. 
It stores the user's authentication ticket (e.g., login status) in a cookie.
 This allows the server to know if a user is authenticated on subsequent requests.
*/
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddJwtBearer()
// .AddCookie(options =>
// {
//     options.Cookie.HttpOnly = true;
//     options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
//     options.LoginPath = "/Auth/Login";
//     options.AccessDeniedPath = "/Auth/AccessDenied";
//     options.SlidingExpiration = true;
// })
.AddCookie("Cookies")
.AddOpenIdConnect("oidc", options =>
{
    options.SignInScheme = "Cookies";
    options.Authority = builder.Configuration["ServiceUrls:AuthAPI"];
    //Claims wil have every detail information
    options.GetClaimsFromUserInfoEndpoint = true;
    //
    options.ClientId = "user";
    options.ClientSecret = StaticDetail.secret;
    options.ResponseType = "code";
    //
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.Scope.Add("api1_scope");
    options.Scope.Add("api2_scope");
    options.Scope.Add("user");
    //
    options.SaveTokens = true;
    // Xử lý claims:
    options.ClaimActions.MapJsonKey("role", "role");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name",
        RoleClaimType = "role",
    };
    //
    options.Events = new OpenIdConnectEvents
    {
        OnRemoteFailure = context =>
            {
                context.Response.Redirect("/");
                context.HandleResponse();
                return Task.CompletedTask;
            },
    };
});

/*
Session: Used to store user-specific data on the server (such as shopping cart information, preferences, etc.).
 The session can be used to store non-authentication-related information temporarily and is tied to the user’s session ID.
 This handles storing session-specific data (like cart items or temporary form data) and sets an idle
*/
// builder.Services.AddSession(options =>
// {
//     options.IdleTimeout = TimeSpan.FromMinutes(100);
//     options.Cookie.HttpOnly = true;
//     options.Cookie.IsEssential = true;
// });
// builder.Services.ConfigureApplicationCookie(options =>
// {
//     options.Cookie.Name = "ids"; // Cùng tên cookie cho cả hai
//     options.Cookie.SameSite = SameSiteMode.Lax;
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRequestTimeout(TimeSpan.FromSeconds(10));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=CoverPage}/{id?}");

app.Run();
