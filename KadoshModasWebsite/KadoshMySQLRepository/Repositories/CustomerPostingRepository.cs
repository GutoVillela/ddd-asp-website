using KadoshDomain.Entities;
using KadoshDomain.Queriables;
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
            return await _dbSet
                .AsNoTracking()
                .Include(CustomerPostingsQueriable.IncludeSale())
                .Where(CustomerPostingsQueriable.GetCustomerPostingsByCustomerId(customerId))
                .OrderByDescending(CustomerPostingsQueriable.OrderByCustomerPostingDate())
                .ToListAsync();
        }

        public async Task<IEnumerable<CustomerPosting>> ReadAllPostingsFromCustomerPaginatedAsync(int customerId, int currentPage, int pageSize)
        {
            int amountToTake = (currentPage - 1) * pageSize;
            return await _dbSet
                .AsNoTracking()
                .Include(CustomerPostingsQueriable.IncludeSale())
                .Where(CustomerPostingsQueriable.GetCustomerPostingsByCustomerId(customerId))
                .OrderByDescending(CustomerPostingsQueriable.OrderByCustomerPostingDate())
                .Skip(amountToTake)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
