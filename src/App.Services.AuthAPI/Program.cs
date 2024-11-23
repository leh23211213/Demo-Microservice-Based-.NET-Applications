using App.Services.AuthAPI;
using App.Services.AuthAPI.Data;
using App.Services.AuthAPI.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppValidate();
builder.AddIdentityServer7(builder.Configuration);
builder.Services.AppServiceCollection(builder.Configuration);
builder.Services.ApiVersionConfiguration();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddRateLimiter();
builder.Services.AddMemoryCache();

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