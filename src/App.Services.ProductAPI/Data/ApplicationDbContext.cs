using App.Services.ProductAPI.Data.Configuration;
using App.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Services.ProductAPI.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }


    public DbSet<Product> Products { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new BrandConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ColorConfiguration());
        modelBuilder.ApplyConfiguration(new SizeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());

        // SeedData
        // Seed data for Brand
        modelBuilder.Entity<Brand>().HasData(
            new Brand { BrandId = 1, BrandName = "Apple", ImageUrl = "https://placehold.co/603x403" }
        );

        // Seed data for Category
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, CategoryName = "Smartphone" }
        );

        // Seed data for Color
        modelBuilder.Entity<Color>().HasData(
            new Color { ColorId = 1, ColorName = "Black" },
            new Color { ColorId = 2, ColorName = "Pink" },
            new Color { ColorId = 3, ColorName = "Red" },
            new Color { ColorId = 4, ColorName = "Blue" },
            new Color { ColorId = 5, ColorName = "Green" }
        );

        // Seed data for Size
        modelBuilder.Entity<Size>().HasData(
            new Size { SizeId = 1, RAM = "128GB" },
            new Size { SizeId = 2, RAM = "256GB" },
            new Size { SizeId = 3, RAM = "512GB" },
            new Size { SizeId = 4, RAM = "1T" }
        );
        //Seed data for Product
        modelBuilder.Entity<Product>().HasData(
            new Product { ProductId = "IP13MiniBK128GB", ProductName = "iPhone 13 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 },
            new Product { ProductId = "IP14MiniBK128GB", ProductName = "iPhone 14 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 },
            new Product { ProductId = "IP15MiniBK128GB", ProductName = "iPhone 15 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 },
            new Product { ProductId = "IP16MiniBK128GB", ProductName = "iPhone 16 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 },
            new Product { ProductId = "IP17MiniBK128GB", ProductName = "iPhone 17 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 },
            new Product { ProductId = "IP18MiniBK128GB", ProductName = "iPhone 18 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 },
            new Product { ProductId = "IP19MiniBK128GB", ProductName = "iPhone 19 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 },
            new Product { ProductId = "IP20MiniBK128GB", ProductName = "iPhone 20 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 },
            new Product { ProductId = "IP21MiniBK128GB", ProductName = "iPhone 21 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 },
            new Product { ProductId = "IP22MiniBK128GB", ProductName = "iPhone 22 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 }
        );
    }
}
