using App.Services.AuthAPI;
using App.Services.AuthAPI.Data;
using App.Services.AuthAPI.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppValidate();
builder.AddIdentityServer7(builder.Configuration); // database + identity server 7 
builder.Services.AppServiceCollection(builder.Configuration);
builder.Services.ApiVersionConfiguration();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddRateLimiter();
// cache
builder.Services.AddDistributedMemoryCache(); // identity
builder.Services.AddMemoryCache(); // rate limit cate

var app = builder.Build();

app.UseSwaggerDocumentation(app.Environment);

//app.UseAntiforgery();

//app.UseIdentityServer();

/* Rate limit*/
app.UseRateLimiter();

// app.MapRazorPages();

app.UseStaticFiles();
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