namespace App.Data.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IProductRepository Product { get; }
        void Save();
    }
}