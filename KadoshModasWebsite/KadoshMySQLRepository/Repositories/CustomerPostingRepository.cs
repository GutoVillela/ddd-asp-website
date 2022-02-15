using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshRepository.Persistence.DataContexts;

namespace KadoshRepository.Repositories
{
    public class CustomerPostingRepository : Repository<CustomerPosting>, ICustomerPostingRepository
    {
        public CustomerPostingRepository(StoreDataContext dbContext) : base(dbContext)
        {
            
        }
    }
}
