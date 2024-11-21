using Stripe;
using App.Services.OrderAPI.Data;
using Microsoft.EntityFrameworkCore;
using App.Services.OrderAPI.Extensions;


var builder = WebApplication.CreateBuilder(args);
builder.AddAppAuthetication();
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.AppServiceCollection(builder.Configuration);

var app = builder.Build();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

//Default
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