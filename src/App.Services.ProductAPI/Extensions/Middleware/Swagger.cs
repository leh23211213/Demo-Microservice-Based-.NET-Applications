using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Versioning;
namespace App.Services.ProductAPI.Extensions
{
    public static class Swagger
    {
        public static IServiceCollection ApiVersionConfiguration(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                options.ReportApiVersions = true;
                //
                // options.ApiVersionReader = ApiVersionReader.Combine(
                //     new UrlSegmentApiVersionReader(),
                //     new HeaderApiVersionReader("X-Api-Version"));
            })
            .AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            })
            .AddEndpointsApiExplorer();

            return services;
        }
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference= new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        }, new string[]{}
                    }
                });
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "App.Services.ProductAPI",
                    Description = "product API version 1",
                    Contact = new OpenApiContact
                    {
                        Name = "Postman Document",
                        Url = new Uri("https://documenter.getpostman.com/view/33236192/2sAXxV5pNK")
                    },
                });
                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2.0",
                    Title = "App.Services.ProductAPI",
                    Description = "product API version 2",
                    Contact = new OpenApiContact
                    {
                        Name = "Postman Document",
                        Url = new Uri("https://documenter.getpostman.com/view/33236192/2sAXxV5pNK")
                    },
                });
            });

            return services;
        }
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                if (env.IsDevelopment())
                {
                    app.UseSwaggerUI(options =>
                    {
                        options.SwaggerEndpoint("/swagger/v1/swagger.json", "App.Services.ProductAPI V1");
                        options.SwaggerEndpoint("/swagger/v2/swagger.json", "App.Services.ProductAPI V2");
                    });
                }
                else
                {
                    app.UseSwaggerUI(options =>
                    {
                        options.SwaggerEndpoint("/swagger/v1/swagger.json", "App.Services.ProductAPI V1");
                        options.SwaggerEndpoint("/swagger/v2/swagger.json", "App.Services.ProductAPI V2");
                        options.RoutePrefix = string.Empty;
                    });
                }
            });
            return app;
        }
    }
}
