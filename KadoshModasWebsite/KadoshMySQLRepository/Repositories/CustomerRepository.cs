using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshRepository.Persistence.DataContexts;

namespace KadoshRepository.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(StoreDataContext dbContext) : base(dbContext)
        {
            
        }
    }
}
