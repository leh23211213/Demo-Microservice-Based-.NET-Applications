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
            entity.Property(e => e.Id).IsRequired();
            entity.Property(e => e.Count).IsRequired();

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
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired(false); ;
            entity.Property(e => e.Phone).HasMaxLength(15).IsRequired(false); ;
            entity.Property(e => e.Email).HasMaxLength(100).IsRequired(false); ;
        });
    }
}
