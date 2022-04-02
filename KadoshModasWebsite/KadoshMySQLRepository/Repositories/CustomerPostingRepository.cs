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

        public async Task<int> CountAllFromCustomerAsync(int customerId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(CustomerPostingsQueriable.GetCustomerPostingsByCustomerId(customerId))
                .CountAsync();
        }

        public async Task<int> CountAllFromSaleAsync(int saleId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(CustomerPostingsQueriable.GetCustomerPostingsBySaleId(saleId))
                .CountAsync();
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

        public async Task<IEnumerable<CustomerPosting>> ReadAllPostingsFromStoreAndDateAsync(DateTime startDateUtc, DateTime endDateUtc, int storeId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(CustomerPostingsQueriable.IncludeSale())
                .Where(CustomerPostingsQueriable.GetCustomerPostingsByStoreAndDate(startDateUtc, endDateUtc, storeId))
                .OrderByDescending(CustomerPostingsQueriable.OrderByCustomerPostingDate())
                .ToListAsync();
        }

        public async Task<IEnumerable<CustomerPosting>> ReadAllPostingsFromStoreAndDatePaginatedAsync(DateTime startDateUtc, DateTime endDateUtc, int storeId, int currentPage, int pageSize)
        {
            int amountToTake = (currentPage - 1) * pageSize;
            return await _dbSet
                .AsNoTracking()
                .Include(CustomerPostingsQueriable.IncludeSale())
                .Where(CustomerPostingsQueriable.GetCustomerPostingsByStoreAndDate(startDateUtc, endDateUtc, storeId))
                .OrderByDescending(CustomerPostingsQueriable.OrderByCustomerPostingDate())
                .Skip(amountToTake)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<CustomerPosting>> ReadAllPostingsFromSaleAsync(int saleId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(CustomerPostingsQueriable.IncludeSale())
                .Where(CustomerPostingsQueriable.GetCustomerPostingsBySaleId(saleId))
                .OrderByDescending(CustomerPostingsQueriable.OrderByCustomerPostingDate())
                .ToListAsync();
        }

        public async Task<IEnumerable<CustomerPosting>> ReadAllPostingsFromSalePaginatedAsync(int saleId, int currentPage, int pageSize)
        {
            int amountToTake = (currentPage - 1) * pageSize;
            return await _dbSet
                .AsNoTracking()
                .Include(CustomerPostingsQueriable.IncludeSale())
                .Where(CustomerPostingsQueriable.GetCustomerPostingsBySaleId(saleId))
                .OrderByDescending(CustomerPostingsQueriable.OrderByCustomerPostingDate())
                .Skip(amountToTake)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
