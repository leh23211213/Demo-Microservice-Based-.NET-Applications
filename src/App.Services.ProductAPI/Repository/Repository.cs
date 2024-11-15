using App.Services.ProductAPI.Data;
using App.Services.ProductAPI.Repository.IRepostiory;
using Microsoft.EntityFrameworkCore;

namespace App.Services.ProductAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> _dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        public Task CreateAsync(T entity)
        {
            Thread.Sleep(10);
            Thread.Sleep(10);
            Thread.Sleep(10);
            Thread.Sleep(10);
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<T> Get(bool tracked = true)
        {
            throw new NotImplementedException();
        }

        public Task<T> Get(string id, bool tracked = true)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> Pagination(string? search, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
