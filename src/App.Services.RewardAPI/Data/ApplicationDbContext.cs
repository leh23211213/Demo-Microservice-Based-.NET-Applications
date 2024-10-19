using App.Services.RewardAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Services.RewardAPI.Extensions
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Rewards> Rewards { get; set; }
    }

}