using KadoshDomain.Entities;
using KadoshShared.Repositories;

namespace KadoshDomain.Repositories
{
    public interface ISaleRepository : IRepository<Sale>
    {
        Task<IEnumerable<Sale>> ReadAllIncludingCustomer();
    }
}
