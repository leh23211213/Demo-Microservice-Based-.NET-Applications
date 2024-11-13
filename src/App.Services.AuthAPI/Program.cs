using App.Services.AuthAPI.Data;
using App.Services.AuthAPI.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppValidate();
builder.AddIdentityServer7(builder.Configuration);
builder.Services.AppServiceCollection(builder.Configuration);
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ApiVersionConfiguration();
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseIdentityServer();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

if (app.Environment.IsDevelopment())
{
    ApplyMigration();
}

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