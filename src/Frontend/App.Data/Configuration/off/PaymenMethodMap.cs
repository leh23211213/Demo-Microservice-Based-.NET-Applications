// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using MVC_Ecommerce.Models;
// namespace Web_MVC_Commerce.Data
// {
//     public class PaymenMethodMap : IEntityTypeConfiguration<PaymentMethod>
//     {
//         public void Configure(EntityTypeBuilder<PaymentMethod> builder)
//         {
//             builder.ToTable("PaymentMethods");
//             builder.HasKey(e => e.PaymentMethodId);
//             builder.Property(e => e.CardNumber).HasMaxLength(20);
//             builder.Property(e => e.ExpiryDate).HasColumnType("DATE");
//             builder.Property(e => e.CVV).HasMaxLength(5);
//             builder.Property(e => e.CardHolderName).HasMaxLength(100);
//             /* 
//                 builder.HasOne<User>(e => e.User)
//                .WithMany()
//                .HasForeignKey(e => e.UserId)
//                .OnDelete(DeleteBehavior.Restrict); // Đảm bảo rằng khi một User bị xóa, PaymentMethod không bị xóa theo
//         builder.HasOne<Address>(e => e.BillingAddress)
//                .WithMany()
//                .HasForeignKey(e => e.BillingAddressId)
//                .OnDelete(DeleteBehavior.Restrict); // Đảm bảo rằng khi một Address bị xóa, PaymentMethod không bị xóa theo
//             */
//         }
//     }
// }