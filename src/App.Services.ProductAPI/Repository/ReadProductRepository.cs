using App.Services.ProductAPI.Data;
using App.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Services.ProductAPI.Repository
{
    public interface IReadProductRepository
    {
        /// <summary>
        /// For other service call
        /// </summary>
        /// <param name="tracked"></param>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetAsync();
        /// <summary>
        /// Get details
        /// </summary>
        /// <returns></returns>
        Task<Product> GetAsync(string id);
        /// <summary>
        /// filter
        /// get
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<IEnumerable<Product>> PaginationAsync(int pageSize, int currentPage, string? search);
    }

    public class ReadProductRepository : IReadProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ReadProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product?>> GetAsync()
        {
            return await _dbContext.Products.AsNoTracking()
                                            .Include(p => p.Size)
                                            .Include(p => p.Category)
                                            .Include(p => p.Color)
                                            .Include(p => p.Brand)
                                            .Select(p => new Product
                                            {
                                                Id = p.Id,
                                                Name = p.Name,
                                                Price = p.Price,
                                                ImageUrl = p.ImageUrl,
                                                ImageLocalPath = p.ImageLocalPath,
                                                Description = p.Description,
                                                Size = p.Size,
                                                Category = p.Category,
                                                Color = p.Color,
                                                Brand = p.Brand,
                                            }).ToListAsync();
        }

        public async Task<Product?> GetAsync(string id)
        {
            return await _dbContext.Products.AsNoTracking()
                                            .Where(p => p.Id == id)
                                            .Include(p => p.Category)
                                            .Include(p => p.Size)
                                            .Include(p => p.Color)
                                            .Include(p => p.Brand)
                                            .Select(p => new Product
                                            {
                                                Id = p.Id,
                                                Name = p.Name,
                                                Price = p.Price,
                                                ImageUrl = p.ImageUrl,
                                                ImageLocalPath = p.ImageLocalPath,
                                                Size = p.Size,
                                                Category = p.Category,
                                                Color = p.Color,
                                                Brand = p.Brand,
                                            }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> PaginationAsync(int pageSize, int currentPage, string? search)
        {
            IQueryable<Product> query;

            query = _dbContext.Products.AsNoTracking()
                                        .Select(p => new Product
                                        {
                                            Id = p.Id,
                                            Name = p.Name,
                                            Price = p.Price,
                                            ImageUrl = p.ImageUrl,
                                            ImageLocalPath = p.ImageLocalPath
                                        });

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.ToLower().Contains(search.ToLower()));
            }

            return await query.ToListAsync();
        }
    }
}
