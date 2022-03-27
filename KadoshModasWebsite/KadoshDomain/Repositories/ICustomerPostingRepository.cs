using KadoshDomain.Entities;
using KadoshShared.Repositories;

namespace KadoshDomain.Repositories
{
    public interface ICustomerPostingRepository : IRepository<CustomerPosting>
    {
        Task<IEnumerable<CustomerPosting>> ReadAllPostingsFromCustomerAsync(int customerId);
        Task<IEnumerable<CustomerPosting>> ReadAllPostingsFromCustomerPaginatedAsync(int customerId, int currentPage, int pageSize);
        Task<IEnumerable<CustomerPosting>> ReadAllPostingsFromSaleAsync(int saleId);
        Task<IEnumerable<CustomerPosting>> ReadAllPostingsFromSalePaginatedAsync(int saleId, int currentPage, int pageSize);
        Task<int> CountAllFromCustomerAsync(int customerId);
        Task<int> CountAllFromSaleAsync(int saleId);
    }
}
