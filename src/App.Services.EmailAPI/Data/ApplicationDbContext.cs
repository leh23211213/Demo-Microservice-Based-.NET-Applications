
using App.Services.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Services.EmailAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<EmailLogger> EmailLoggers { get; set; }
    }
}