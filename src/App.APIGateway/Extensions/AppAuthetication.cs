namespace App.APIGateway.Extensions
{
    public static class AppAuthetication
    {
        public static WebApplicationBuilder AddAppAuthetication(this WebApplicationBuilder builder)
        {
            var authrityUrl = builder.Configuration.GetValue<string>("ServiceUrls:AuthAPI");

            var settingsSection = builder.Configuration.GetSection("ApiSettings");
            var secret = settingsSection.GetValue<string>("Secret");
            var issuer = settingsSection.GetValue<string>("Issuer");
            var audience = settingsSection.GetValue<string>("Audience");
            //var key = Convert.FromBase64String(secret); 500.30
            var key = System.Text.Encoding.UTF8.GetBytes(secret);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // Enable if you're not using HTTPS (for dev environment)
                options.SaveToken = true;
                options.Authority = authrityUrl;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero,
                };
            });

<<<<<<<< HEAD:src/App.Services.ProductAPI/Extensions/AppAuthetication.cs
            builder.Services.AddAuthorization(options =>
            {
            });
========
            builder.Services.AddAuthorization();
>>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/App.APIGateway/Extensions/AppAuthetication.cs
            builder.Services.AddControllers();

            return builder;
        }
    }
}
