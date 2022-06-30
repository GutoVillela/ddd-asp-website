using KadoshDomain.Entities;
using KadoshShared.Repositories;

namespace KadoshDomain.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> ReadAllByNameAsync(string productName);
        Task<IEnumerable<Product>> ReadAllByNamePagedAsync(string productName, int currentPage, int pageSize);
        Task<int> CountAllByNameAsync(string productName);
        Task<Product?> ReadByBarCodeAsync(string barCode);
    }
}
