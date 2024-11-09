using App.Frontend.Utility;
using App.Frontend.Services;
using App.Frontend.Extensions;
using App.Frontend.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

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

/*
Cookie Authentication: Used to manage user authentication. 
It stores the user's authentication ticket (e.g., login status) in a cookie.
 This allows the server to know if a user is authenticated on subsequent requests.
*/
builder.Logging.AddConsole();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "oidc";
})
.AddJwtBearer()
.AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
})
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = builder.Configuration["ServiceUrls:AuthAPI"];
    //Claims wil have every detail information
    options.GetClaimsFromUserInfoEndpoint = true;
    //
    options.ClientId = "levanhiep409159";
    options.ClientSecret = StaticDetail.secret;
    options.ResponseType = "code";
    //
    options.TokenValidationParameters.NameClaimType = "name";
    options.TokenValidationParameters.RoleClaimType = "role";
    //
    options.Scope.Add("levanhiep409159");
    options.SaveTokens = true;

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
/*
Session: Used to store user-specific data on the server (such as shopping cart information, preferences, etc.).
 The session can be used to store non-authentication-related information temporarily and is tied to the user’s session ID.
 This handles storing session-specific data (like cart items or temporary form data) and sets an idle
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=CoverPage}/{id?}");

app.Run();
