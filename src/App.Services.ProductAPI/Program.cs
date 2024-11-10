using App.Services.ProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using App.Services.ProductAPI.Extensions;
using AspNetCoreRateLimit;

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
builder.Services.AddInMemoryRateLimiting();
var app = builder.Build();
// app.UseMiddleware<RateLimitMiddleware>();

app.UseIpRateLimiting();

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

app.UseSwaggerDocumentation(app.Environment);

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