using App.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Services.OrderAPI.Data;
public class ApplicationDbContext : DbContext
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
  {
  }

  public DbSet<OrderHeader> OrderHeaders { get; set; }
  public DbSet<OrderDetails> OrderDetails { get; set; }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<OrderHeader>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .IsRequired();
      entity.Property(e => e.Name)
                .IsRequired(false);
      entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsRequired(false);
      entity.Property(e => e.Email)
                .HasMaxLength(256)
                .IsRequired(false);

      entity.Property(e => e.CouponCode)
                .HasMaxLength(50)
                .IsRequired(false);

      entity.Property(e => e.Discount)
                .HasColumnType("decimal(18,2)");

      entity.Property(e => e.OrderTotal)
                .HasColumnType("decimal(18,2)");
      entity.Property(e => e.OrderTime)
                .HasColumnType("datetime");

      entity.Property(e => e.Status)
                  .IsRequired(false);
      entity.Property(e => e.PaymentIntentId)
                   .IsRequired(false);
      entity.Property(e => e.StripeSessionId)
                  .IsRequired(false);
      entity.Ignore(e => e.OrderDetails);
    });

    modelBuilder.Entity<OrderDetails>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.Property(e => e.ProductId)
                .HasMaxLength(100)
                .IsRequired();

      entity.Property(e => e.ProductName)
                .HasMaxLength(255)
               .IsRequired(false);

      entity.Property(e => e.Price)
                .HasColumnType("decimal(18,2)");

      entity.HasOne<OrderHeader>()
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(d => d.OrderHeaderId)
                .OnDelete(DeleteBehavior.Cascade);
      entity.Ignore(e => e.Product);
    });
  }
}
