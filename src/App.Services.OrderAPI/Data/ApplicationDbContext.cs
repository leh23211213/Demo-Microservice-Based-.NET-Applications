using App.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Services.OrderAPI.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderHeader>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UserId)
                  .HasMaxLength(100)
                  .IsRequired(false);

            entity.Property(e => e.CouponCode)
                  .HasMaxLength(50)
                  .IsRequired(false);

            entity.Property(e => e.Discount)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(e => e.OrderTotal)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(e => e.OrderTime)
                  .HasColumnType("datetime");

            entity.Ignore(e => e.OrderDetails);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.ProductId)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(e => e.Count)
                  .IsRequired();

            entity.Property(e => e.Price)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(e => e.ProductName)
                  .HasMaxLength(255)
                  .IsRequired();


            entity.HasOne<OrderHeader>()
                  .WithMany(o => o.OrderDetails)
                  .HasForeignKey(d => d.OrderHeaderId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Ignore(e => e.product);
        });
    }

}
