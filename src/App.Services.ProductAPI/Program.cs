using AspNetCoreRateLimit;
using App.Services.ProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using App.Services.ProductAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppAuthetication();
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.AppServiceCollection(builder.Configuration);
builder.Services.ApiVersionConfiguration();
builder.Services.AddSwaggerDocumentation();
// Third-Party Libraries
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*",
            Limit = 10, // Sets the maximum number of requests.
            Period = "10s" // Defines the time period (e.g., 10s for 10 seconds).
        }
    };
});

builder.Services.AddMemoryCache();
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

var app = builder.Build();

app.UseIpRateLimiting();
app.UseRouting();

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