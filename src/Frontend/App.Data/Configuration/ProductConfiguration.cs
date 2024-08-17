using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App.Data.Models;

namespace App.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(e => e.ProductId);
            builder.Property(e => e.ProductName).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Description).IsRequired(false).HasMaxLength(500); ;
            builder.Property(e => e.Price).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(e => e.Quantity).IsRequired(false);
            builder.Property(e => e.ImageUrl).HasMaxLength(255).IsRequired(false);
            builder.Property(e => e.Vote).IsRequired(false);

            // Định nghĩa khóa ngoại
            builder.HasMany(p => p.CartItems)
            .WithOne(ci => ci.Product)
            .HasForeignKey(ci => ci.ProductId);

            builder.HasOne(p => p.Brand)
               .WithMany(b => b.Products)
               .HasForeignKey(p => p.BrandId);

            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId);
        }
    }
}