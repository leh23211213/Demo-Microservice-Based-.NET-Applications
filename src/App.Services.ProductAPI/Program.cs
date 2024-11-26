using App.Services.ProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using App.Services.ProductAPI.Extensions;
using App.Services.ProductAPI;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppAuthetication();
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.AppServiceCollection(builder.Configuration);
builder.Services.ApiVersionConfiguration();
builder.Services.AddSwaggerDocumentation();

builder.Services.AddRateLimiter();
// cache
builder.Services.AddDistributedMemoryCache(); // identity
builder.Services.AddMemoryCache(); // rate limit cate

var app = builder.Build();

/* Rate limit*/
app.UseRateLimiter();

// Chỉ bật Swagger khi không chạy benchmark
// var isBenchmarkMode = Environment.GetEnvironmentVariable("BENCHMARK_MODE") == "true";

// if (!isBenchmarkMode)
// {
//      BenchmarkRunner.Run<ProductControllerBenchmark>();
// }

app.UseSwaggerDocumentation(app.Environment);

app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

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