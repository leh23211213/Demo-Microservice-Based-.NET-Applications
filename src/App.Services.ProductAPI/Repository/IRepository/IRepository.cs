namespace App.Services.ProductAPI.Repository.IRepostiory
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> Pagination(string? search, int pageSize, int pageNumber);
        Task<T> Get(bool tracked = true);
        Task<T> Get(string id, bool tracked = true);
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task DeleteAsync(string id);
        Task SaveAsync();
    }
}
