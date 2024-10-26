using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using template.Extensions;
using template.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.AppServiceCollection(builder.Configuration);

builder.Services.AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
    });

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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

    option.SwaggerDoc("v1", new OpenApiInfo
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
    option.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2.0",
        Title = "App.Services.ProductAPI",
    });
});

builder.AddAppAuthetication();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    if (app.Environment.IsDevelopment())
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

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
ApplyMigration(app);
app.Run();

void ApplyMigration(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}