using KadoshDomain.Entities;
using KadoshShared.Repositories;

namespace KadoshDomain.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<IEnumerable<Customer>> ReadAllByNameAsync(string customerName);
        Task<IEnumerable<Customer>> ReadAllByNamePaginatedAsync(string customerName, int currentPage, int pageSize);
        Task<int> CountAllByNameAsync(string customerName);
    }
}
