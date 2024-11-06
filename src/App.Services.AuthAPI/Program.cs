using App.Services.AuthAPI.Data;
using App.Services.AuthAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppAuthetication();
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.AppServiceCollection(builder.Configuration);

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1.0",
        Title = "App.Services.AuthAPI",
    });
});

builder.Services.AddDistributedMemoryCache();

//30-1-2024
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowSpecificOrigins", builder =>
//     {
//         builder.WithOrigins("http://localhost:6999")  // replace with your frontend URL
//                .AllowAnyMethod()
//                .AllowAnyHeader()
//                .AllowCredentials(); // only if needed for cookies or credentials
//     });
// });

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(options =>
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "App.Services.AuthAPI V1");
        });
    }
    else
    {
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "App.Services.AuthAPI V1");
            options.RoutePrefix = string.Empty;
        });
    }
});

//30-1-2024
// app.UseCors("AllowSpecificOrigins");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
ApplyMigration();
app.Run();

void ApplyMigration()
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