using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App.Services.ProductAPI.Models;

namespace App.Services.ProductAPI.Data.Configuration
{
       public class ProductConfiguration : IEntityTypeConfiguration<Product>
       {
              public void Configure(EntityTypeBuilder<Product> builder)
              {
                     builder.ToTable("Products");
                     builder.HasKey(e => e.Id);

                     builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
                     builder.Property(e => e.Price).HasColumnType("decimal(18,2)").IsRequired();
                     builder.Property(e => e.ImageUrl).HasMaxLength(255).IsRequired(false);

                     builder.HasOne(p => p.Size)
                            .WithMany(s => s.Products)
                            .HasForeignKey(p => p.SizeId)
                            .OnDelete(DeleteBehavior.Cascade);

                     builder.HasOne(p => p.Color)
                            .WithMany(c => c.Products)
                            .HasForeignKey(p => p.ColorId)
                            .OnDelete(DeleteBehavior.Cascade);

                     builder.HasOne(p => p.Category)
                            .WithMany(c => c.Products)
                            .HasForeignKey(p => p.CategoryId)
                            .OnDelete(DeleteBehavior.Cascade);

                     builder.HasOne(p => p.Brand)
                            .WithMany(b => b.Products)
                            .HasForeignKey(p => p.BrandId)
                            .OnDelete(DeleteBehavior.Cascade);
              }
       }
}