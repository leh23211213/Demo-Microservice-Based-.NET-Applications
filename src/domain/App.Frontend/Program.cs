using App.Frontend.Utility;
using App.Frontend.Services;
using App.Frontend.Extensions;
using App.Frontend.Services.IServices;
using Microsoft.AspNetCore.Authentication;

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
    options.ClientId = "user_scope";
    options.ClientSecret = StaticDetail.secret;
    options.ResponseType = "code";
    options.SignInScheme = "Cookies";
    //  
    options.TokenValidationParameters.NameClaimType = "name";
    options.TokenValidationParameters.RoleClaimType = "role";
    //
    options.Scope.Add("user_scope");
    options.SaveTokens = true;
    options.RequireHttpsMetadata = false;
    options.ClaimActions.MapJsonKey("role", "role");
    options.Events = new OpenIdConnectEvents
    {
        OnRemoteFailure = context =>
        {
            context.Response.Redirect("/");
            context.HandleResponse(); // Chặn xử lý mặc định và thực hiện điều hướng tùy chỉnh
            return Task.FromResult(0);
        },
        OnTokenValidated = context =>
        {
            // Redirect to a specific action after successful login
            context.Response.Redirect("/user/login");
            context.HandleResponse();
            return Task.CompletedTask;
        }

    };
    options.CallbackPath = new PathString("/Account/Authentication/signin-oidc");
    options.RemoteSignOutPath = new PathString("/Account/Authentication/signout-oidc");
    options.SignedOutCallbackPath = new PathString("/Account/Authentication/signout-callback-oidc");
});


/*
Session: Used to store user-specific data on the server (such as shopping cart information, preferences, etc.).
 The session can be used to store non-authentication-related information temporarily and is tied to the user’s session ID.
 This handles storing session-specific data (like cart items or temporary form data) and sets an idle
*/
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "ids"; // Cùng tên cookie cho cả hai
    options.Cookie.SameSite = SameSiteMode.Lax;
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapControllerRoute(
//         name: "areas",
//         pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
//     );

// });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=coverPage}/{id?}");

app.Run();
