using App.Data.Configuration;
using App.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.Data;
public class ApplicationDbContext :  IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductDetail> ProductDetails { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
        modelBuilder.ApplyConfiguration(new CartConfiguration());
        modelBuilder.ApplyConfiguration(new BrandConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ColorConfiguration());
        modelBuilder.ApplyConfiguration(new SizeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ProductDetailConfiguration());

        // SeedData
        // Seed data for Brand
        modelBuilder.Entity<Brand>().HasData(
            new Brand { BrandId = 1, BrandName = "Apple", ImageUrl = "~/lib/image/Brands/Apple.png" }
        );

        // Seed data for Category
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, CategoryName = "Smartphone" }
        );

        // Seed data for Color
        modelBuilder.Entity<Color>().HasData(
            new Color { ColorId = 1, ColorName = "Black" },
            new Color { ColorId = 2, ColorName = "Pink" }
        );

        // Seed data for Size
        modelBuilder.Entity<Size>().HasData(
            new Size { SizeId = 1, SizeName = "128GB" },
            new Size { SizeId = 2, SizeName = "256GB" }
        );
        // Seed data for Product
        modelBuilder.Entity<Product>().HasData(
            new Product { ProductId = "IP13MiniBK128GB", ProductName = "iPhone 13 Mini", Price = 999, ImageUrl = "~/lib/image/SmartPhone/Iphone/IP13-Mini-BK-128GB.png", BrandId = 1, CategoryId = 1 },
            new Product { ProductId = "IP13MiniPK128GB", ProductName = "iPhone 13 Mini", Price = 999, ImageUrl = "~/lib/image/SmartPhone/Iphone/IP13-Mini-PK-128GB.png", BrandId = 1, CategoryId = 1 }
        );

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