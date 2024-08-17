using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App.Data.Models;

namespace App.Data.Configuration
{
       public class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
       {
              public void Configure(EntityTypeBuilder<ProductDetail> builder)
              {
                     builder.HasKey(pd => pd.ProductDetailId);
                     builder.HasOne(pd => pd.Product)
                            .WithMany(p => p.ProductDetails)
                            .HasForeignKey(pd => pd.ProductId);

                     builder.HasOne(pd => pd.Color)
                            .WithMany(c => c.ProductDetails)
                            .HasForeignKey(pd => pd.ColorId);

                     builder.HasOne(pd => pd.Size)
                            .WithMany(s => s.ProductDetails)
                            .HasForeignKey(pd => pd.SizeId);
              }
       }
}
