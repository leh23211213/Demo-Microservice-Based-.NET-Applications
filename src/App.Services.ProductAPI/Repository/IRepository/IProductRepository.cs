namespace App.Services.ProductAPI.Repository.IRepostiory
{
    public interface IProductRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(string? search, int pageSize = 0, int pageNumber = 1);
        Task<T> GetAsync(bool tracked = true);
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task SaveAsync();
    }
}
