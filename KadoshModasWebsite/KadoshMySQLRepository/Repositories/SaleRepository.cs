using KadoshDomain.Entities;
using KadoshDomain.Queries;
using KadoshDomain.Repositories;
using KadoshRepository.Persistence.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace KadoshRepository.Repositories
{
    public class SaleRepository : Repository<Sale>, ISaleRepository
    {
        public SaleRepository(StoreDataContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<IEnumerable<Sale>> ReadAllIncludingCustomerAsync()
        {
            return await _dbSet.AsNoTracking().Include(SaleQueries.IncludeCustomer()).Include(SaleQueries.IncludeSaleItems()).ToListAsync();
        }

        public async Task<IEnumerable<Sale>> ReadAllFromCustomerAsync(int customerId)
        {
            return await _dbSet.AsNoTracking()
                .Include(SaleQueries.IncludeCustomer())
                .Include(SaleQueries.IncludeSaleItems())
                .Where(SaleQueries.GetSalesByCustomer(customerId))
                .ToListAsync();
        }
    }
}
