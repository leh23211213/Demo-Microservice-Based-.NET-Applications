using App.Services.ProductAPI.Data;
using App.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Services.ProductAPI.Repository
{
    public interface IWriteProductRepository
    {
        Task RemoveAsync(Product entity);
        Task DeleteAsync(string id);
        Task SaveAsync();
    }
    public class WriteProductRepository : IWriteProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        internal DbSet<Product> dbSet;
        public WriteProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            this.dbSet = dbContext.Set<Product>();
        }

        public async Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
