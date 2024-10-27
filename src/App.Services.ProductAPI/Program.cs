using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using App.Services.ProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using App.Services.ProductAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.AppServiceCollection(builder.Configuration);

builder.Services.AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        // options.ApiVersionReader = new UrlSegmentApiVersionReader();  // Read version from URL
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
        });
    }
    else
    {
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "App.Services.ProductAPI V1");
            options.RoutePrefix = string.Empty;
        });
    }
});

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    ApplyMigration(app);
}


app.Run();


// 500.30
void ApplyMigration(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        // Log and handle the migration error
        logger.LogError(ex, "An error occurred while applying migrations.");
        // Optional: Handle the error (e.g., rethrow or notify)
    }
}