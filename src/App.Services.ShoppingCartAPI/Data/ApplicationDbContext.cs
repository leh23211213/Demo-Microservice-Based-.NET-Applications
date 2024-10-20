using App.Services.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Services.ShoppingCartAPI.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<CartDetails> CartDetails { get; set; }
    public DbSet<CartHeader> CartHeaders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CartDetails>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Count).IsRequired();

            entity.Ignore(e => e.CartHeader);
            entity.Ignore(e => e.Product);

            entity.HasOne(p => p.CartHeader)
                           .WithMany(s => s.CartDetails)
                           .HasForeignKey(p => p.CartHeaderId)
                           .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CartHeader>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).IsRequired();
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.CouponCode).IsRequired(false);
            entity.Ignore(e => e.Discount);
            entity.Ignore(e => e.Total);
            entity.Ignore(e => e.Count);
            entity.Ignore(e => e.Name);
            entity.Ignore(e => e.Phone);
            entity.Ignore(e => e.Email);
            entity.Ignore(e => e.CartDetails);
        });
    }
}
