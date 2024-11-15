using App.Frontend.Utility;
using App.Frontend.Services;
using App.Frontend.Extensions;
using Microsoft.AspNetCore.Authentication;
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
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews(u => u.Filters.Add(new AuthExceptionRedirection()));

/*
Cookie Authentication: Used to manage user authentication. 
It stores the user's authentication ticket (e.g., login status) in a cookie.
 This allows the server to know if a user is authenticated on subsequent requests.
*/

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "OpenIdConnect";
})
.AddJwtBearer()
.AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromSeconds(20);
    options.LoginPath = "/Account/Authentication/Login";
    options.AccessDeniedPath = "/Account/Authentication/AccessDenied";
})
.AddOpenIdConnect("OpenIdConnect", options =>
{
    //Claims wil have every detail information
    //options.SignInScheme = "Cookies";
    options.Authority = builder.Configuration["ServiceUrls:AuthAPI"];
    //options.GetClaimsFromUserInfoEndpoint = true;
    //
    options.ClientId = "user_scope";
    options.ClientSecret = StaticDetail.secret;
    options.ResponseType = "code";
    //  
    //options.TokenValidationParameters.NameClaimType = "name";
    //options.TokenValidationParameters.RoleClaimType = "role";
    //
    options.Scope.Add("user_scope");
    //
    //options.SaveTokens = true;
    //options.RequireHttpsMetadata = false;
    options.ClaimActions.MapJsonKey("role", "role");
    //
    options.Events = new OpenIdConnectEvents
    {
        OnRemoteFailure = context =>
            {
                context.Response.Redirect("/");
                context.HandleResponse(); // Chặn xử lý mặc định và thực hiện điều hướng tùy chỉnh
                return Task.CompletedTask;
            },
        OnAccessDenied = context =>
            {
                context.Response.Redirect("/AccessDenied");
                context.HandleResponse(); // Chặn xử lý mặc định và thực hiện điều hướng tùy chỉnh
                return Task.CompletedTask;
            },
        OnUserInformationReceived = context =>
            {
                context.Response.Redirect("/Account/Authentication/Login");
                context.HandleResponse(); // Chặn xử lý mặc định và thực hiện điều hướng tùy chỉnh
                return Task.CompletedTask;
            },
        OnTokenResponseReceived = context =>
            {
                context.Response.Redirect("/Account/Authentication/Login");
                context.HandleResponse(); // Chặn xử lý mặc định và thực hiện điều hướng tùy chỉnh
                return Task.CompletedTask;
            },
        OnTokenValidated = context =>
            {
                context.Response.Redirect("/Account/Authentication/Login");
                context.HandleResponse(); // Chặn xử lý mặc định và thực hiện điều hướng tùy chỉnh
                return Task.CompletedTask;
            },
    };
    options.CallbackPath = new PathString("/Account/Authentication/signin-oidc");
    options.SignedOutCallbackPath = new PathString("/Account/Authentication/signout-callback-oidc");
});

/*
Session: Used to store user-specific data on the server (such as shopping cart information, preferences, etc.).
 The session can be used to store non-authentication-related information temporarily and is tied to the user’s session ID.
 This handles storing session-specific data (like cart items or temporary form data) and sets an idle
*/
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(7);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;  // Make the session cookie essential

});


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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=CoverPage}/{id?}");

app.Run();
