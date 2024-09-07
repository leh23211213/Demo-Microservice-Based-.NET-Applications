using App.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.Services.AuthAPI.Data;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }
    }
}

/*
Scaffold-DbContext 
"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=ApplicationDbContext-62d68e45-c6a4-4a8e-8a7e-c9b067c2823d;Integrated Security=True;Connect
 Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False" Microsoft.EntityFrameworkCore.SqlServer -ContextDir Data -Outputir Models

dotnet ef dbcontext scaffold "Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=ApplicationDbContext-62d68e45-c6a4-4a8e-8a7e-c9b067c2823d;Integrated Security=True;Conn
ect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False" Microsoft.EntityFrameworkCore.SqlServer --context-dir Data --output-dir Models
*/