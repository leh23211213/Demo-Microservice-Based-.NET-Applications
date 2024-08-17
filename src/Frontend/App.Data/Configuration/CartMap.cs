using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App.Data.Models;

namespace App.Data.Configuration
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carts");
            builder.HasKey(c => c.CartId);

            builder.Property(c => c.UserId)
                .IsRequired();

            builder.Property(c => c.DateCreated)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId);

            // Cấu hình mối quan hệ giữa Cart và User
            builder
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Hoặc DeleteBehavior.Restrict nếu bạn không muốn xóa tự động
        }
    }
}
