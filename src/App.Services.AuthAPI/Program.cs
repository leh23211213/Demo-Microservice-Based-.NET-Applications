using App.Services.AuthAPI.Data;
using App.Services.AuthAPI.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
<<<<<<< HEAD
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
=======
builder.AddAppValidate();
builder.AddIdentityServer7(builder.Configuration);
builder.Services.AppServiceCollection(builder.Configuration);
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ApiVersionConfiguration();
// builder.Services.AddSwaggerDocumentation();
builder.Services.AddDistributedMemoryCache();
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0

builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// app.UseSwaggerDocumentation(app.Environment);


app.UseSession();
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