using KadoshDomain.Entities;
using KadoshShared.Repositories;

namespace KadoshDomain.Repositories
{
    public interface ICustomerPostingRepository : IRepository<CustomerPosting>
    {
        Task<IEnumerable<CustomerPosting>> ReadAllPostingsFromCustomerAsync(int customerId);
    }
}
