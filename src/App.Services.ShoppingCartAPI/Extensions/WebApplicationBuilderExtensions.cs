<<<<<<< HEAD:src/App.Services.OrderAPI/Extensions/WebApplicationBuilderExtensions.cs
﻿namespace App.Services.OrderAPI.Extensions
=======
﻿namespace App.Services.ShoppingCartAPI.Extensions
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/App.Services.ShoppingCartAPI/Extensions/AppAuthetication.cs
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddAppAuthetication(this WebApplicationBuilder builder)
        {
            var settingsSection = builder.Configuration.GetSection("ApiSettings");

            var secret = settingsSection.GetValue<string>("Secret");
            var issuer = settingsSection.GetValue<string>("Issuer");
            var audience = settingsSection.GetValue<string>("Audience");
<<<<<<< HEAD:src/App.Services.OrderAPI/Extensions/WebApplicationBuilderExtensions.cs
            var key = Convert.FromBase64String(secret);
=======
            //var key = Convert.FromBase64String(secret); 500.30
            var key = System.Text.Encoding.UTF8.GetBytes(secret);
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/App.Services.ShoppingCartAPI/Extensions/AppAuthetication.cs

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
<<<<<<< HEAD:src/App.Services.OrderAPI/Extensions/WebApplicationBuilderExtensions.cs
                options.DefaultScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
=======
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/App.Services.ShoppingCartAPI/Extensions/AppAuthetication.cs
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // Enable if you're not using HTTPS (for dev environment)
                options.SaveToken = true;
<<<<<<< HEAD:src/App.Services.OrderAPI/Extensions/WebApplicationBuilderExtensions.cs
=======
                options.Authority = authrityUrl;
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/App.Services.ShoppingCartAPI/Extensions/AppAuthetication.cs
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
<<<<<<< HEAD:src/App.Services.OrderAPI/Extensions/WebApplicationBuilderExtensions.cs
                };
            });

            builder.Services.AddAuthorization();
=======
                    ClockSkew = TimeSpan.Zero,
                };
            });

            builder.Services.AddAuthorization(options =>
            {
            });
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/App.Services.ShoppingCartAPI/Extensions/AppAuthetication.cs
            builder.Services.AddControllers();
            builder.Services.AddAuthentication();
            return builder;
        }
    }
}
