namespace App.Services.ProductAPI.Repository.IRepostiory
{
    public interface IProductRepository<T> where T : class
    {
        Task<List<T>> Pagination(string? search, int pageSize, int pageNumber);
        /// <summary>
        /// For other service call
        /// </summary>
        /// <param name="tracked"></param>
        /// <returns></returns>
        Task<T> Get(bool tracked = true);
        /// <summary>
        /// Get detailt
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tracked"></param>
        /// <returns></returns>
        Task<T> Get(string id, bool tracked = true);
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task DeleteAsync(string id);
        Task SaveAsync();
    }
}
