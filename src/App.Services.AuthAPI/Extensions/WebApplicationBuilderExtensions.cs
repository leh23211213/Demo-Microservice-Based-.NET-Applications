namespace App.Services.AuthAPI.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddAppAuthetication(this WebApplicationBuilder builder)
        {
            var settingsSection = builder.Configuration.GetSection("ApiSettings");

            var secret = settingsSection.GetValue<string>("Secret");
            var issuer = settingsSection.GetValue<string>("Issuer");
            var audience = settingsSection.GetValue<string>("Audience");
            var key = Convert.FromBase64String(secret);

            builder.Services.AddAuthentication(options =>
            {
                /*
                    Setting DefaultAuthenticateScheme to JwtBearerDefaults.AuthenticationScheme tells ASP.NET Core to primarily use JWT Bearer tokens for authenticating users by default.
                    This is useful for API calls where you expect the Authorization header to contain a JWT token.
                */
                options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                /*
                    Setting DefaultChallengeScheme to JwtBearerDefaults.AuthenticationScheme means that when authentication is required and fails 
                    (e.g., the token is missing or invalid), the application will respond with a challenge specific to JWT Bearer authentication.
                    For APIs, this would typically return a 401 Unauthorized response.
                */
                options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // Enable if you're not using HTTPS (for dev environment)
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                };
            }).AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(7); // the expiration time for the authentication cookie
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.HttpOnly = true;
                options.SlidingExpiration = true;
            }); ;

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(7);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;  // Make the session cookie essential
            });

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            builder.Services.AddAuthentication();
            return builder;
        }
    }
}
