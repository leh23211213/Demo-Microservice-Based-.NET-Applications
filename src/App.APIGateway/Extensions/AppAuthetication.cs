namespace App.APIGateway
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
            // var key = Convert.FromBase64String(secret);// 500.30
            var key = System.Text.Encoding.UTF8.GetBytes(secret);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            })
            .AddJwtBearer(options =>
            {
                // SSO
                // options.RequireHttpsMetadata = false; // Enable if you're not using HTTPS (for dev environment)
                // options.SaveToken = true;
                // options.Authority = authrityUrl;
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

            builder.Services.AddAuthorization(options =>
            {
            });
            builder.Services.AddControllers();

            return builder;
        }
    }
}
