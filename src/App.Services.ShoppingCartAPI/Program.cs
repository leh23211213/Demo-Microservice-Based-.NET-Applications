using Microsoft.EntityFrameworkCore;
using App.Services.ShoppingCartAPI.Data;
using App.Services.ShoppingCartAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppAuthentication();
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.AppServiceCollection(builder.Configuration);
builder.Services.ApiVersionConfiguration();
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

app.UseSwaggerDocumentation(app.Environment);

//Default;
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
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