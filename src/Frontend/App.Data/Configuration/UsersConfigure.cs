using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App.Data.Models;

namespace App.Data.Configuration
{
    class UsersConfigure : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            // Thiết lập cấu hình cho thuộc tính HomeAdress
            // builder.Property(e => e.AddressLine1)
            // .IsRequired(false) // (Tuỳ chọn) Có hoặc không cần thiết
            // .HasColumnType("nvarchar") // Kiểu dữ liệu cột
            // .HasMaxLength(400); // Độ dài tối đa
            // builder.Property(e => e.AddressLine2)
            // .IsRequired(false)
            // .HasColumnType("nvarchar")
            // .HasMaxLength(400);
            // Thiết lập cấu hình cho thuộc tính BirthDate
            builder.Property(e => e.BirthDate)
           .IsRequired(false) // (Tuỳ chọn) Có hoặc không cần thiết
           .HasColumnType("date"); // Kiểu dữ liệu cột

            // Cấu hình mối quan hệ giữa User và Cart
            builder.HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Hoặc DeleteBehavior.Restrict nếu bạn không muốn xóa tự động
        }
    }
}