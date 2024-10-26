using App.Services.ProductAPI.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using template.Models;

namespace template.Data;
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
            new Brand { Id = 1, Name = "Apple", ImageUrl = "https://placehold.co/603x403" }
        );

        // Seed data for Category
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Smartphone" }
        );

        // Seed data for Color
        modelBuilder.Entity<Color>().HasData(
            new Color { Id = 1, Name = "Black" },
            new Color { Id = 2, Name = "Pink" },
            new Color { Id = 3, Name = "Red" },
            new Color { Id = 4, Name = "Blue" },
            new Color { Id = 5, Name = "Green" }
        );

        // Seed data for Size
        modelBuilder.Entity<Size>().HasData(
            new Size { Id = 1, RAM = "128GB" },
            new Size { Id = 2, RAM = "256GB" },
            new Size { Id = 3, RAM = "512GB" },
            new Size { Id = 4, RAM = "1T" }
        );
        //Seed data for Product
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = "IP13MiniBK128GB", Name = "iPhone 13 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 },
            new Product { Id = "IP14MiniBK128GB", Name = "iPhone 14 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 },
            new Product { Id = "IP15MiniBK128GB", Name = "iPhone 15 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 },
            new Product { Id = "IP16MiniBK128GB", Name = "iPhone 16 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 },
            new Product { Id = "IP17MiniBK128GB", Name = "iPhone 17 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 },
            new Product { Id = "IP18MiniBK128GB", Name = "iPhone 18 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 },
            new Product { Id = "IP19MiniBK128GB", Name = "iPhone 19 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 },
            new Product { Id = "IP20MiniBK128GB", Name = "iPhone 20 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 },
            new Product { Id = "IP21MiniBK128GB", Name = "iPhone 21 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 },
            new Product { Id = "IP22MiniBK128GB", Name = "iPhone 22 Mini", Price = 999, ImageUrl = "https://placehold.co/603x403", BrandId = 1, CategoryId = 1, ColorId = 1, SizeId = 1 }
        );
    }
}
