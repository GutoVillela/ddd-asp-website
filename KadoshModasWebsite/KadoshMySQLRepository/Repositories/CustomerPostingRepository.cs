using KadoshDomain.Entities;
using KadoshDomain.Queries;
using KadoshDomain.Repositories;
using KadoshRepository.Persistence.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace KadoshRepository.Repositories
{
    public class CustomerPostingRepository : Repository<CustomerPosting>, ICustomerPostingRepository
    {
        public CustomerPostingRepository(StoreDataContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<IEnumerable<CustomerPosting>> ReadAllPostingsFromCustomerAsync(int customerId)
        {
            return await _dbSet.AsNoTracking()
                .Include(CustomerPostingsQueries.IncludeSale()).AsNoTracking()
                .Where(CustomerPostingsQueries.GetCustomerPostingsByCustomerId(customerId)).ToListAsync();
        }
    }
}
