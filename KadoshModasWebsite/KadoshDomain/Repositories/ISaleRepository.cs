using KadoshDomain.Entities;
using KadoshShared.Repositories;

namespace KadoshDomain.Repositories
{
    public interface ISaleRepository : IRepository<Sale>
    {
        Task<IEnumerable<Sale>> ReadAllIncludingCustomerAsync();
        Task<IEnumerable<Sale>> ReadAllFromCustomerAsync(int customerId);
    }
}
