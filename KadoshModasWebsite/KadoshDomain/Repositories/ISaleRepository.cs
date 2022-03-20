using KadoshDomain.Entities;
using KadoshShared.Repositories;

namespace KadoshDomain.Repositories
{
    public interface ISaleRepository : IRepository<Sale>
    {
        Task<IEnumerable<Sale>> ReadAllIncludingCustomerAsync();
        Task<IEnumerable<Sale>> ReadAllFromCustomerAsync(int customerId);
        Task<IEnumerable<Sale>> ReadAllOpenFromCustomerAsync(int customerId);
        Task<IEnumerable<Sale>> ReadAllFromCustomerPaginatedAsync(int customerId, int currentPage, int pageSize);
        Task<int> CountAllFromCustomerAsync(int customerId);
    }
}
