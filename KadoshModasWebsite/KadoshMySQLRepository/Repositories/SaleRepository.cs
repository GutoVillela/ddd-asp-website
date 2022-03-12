using KadoshDomain.Entities;
using KadoshDomain.Enums;
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

        public override async Task<IEnumerable<Sale>> ReadAllAsync()
        {
            return await _dbSet.AsNoTracking()
                .Include(SaleQueries.IncludeSaleItems())
                .Include(SaleQueries.IncludePostings())
                .OrderByDescending(SaleQueries.OrderBySaleDate())
                .ToListAsync();
        }

        public override async Task<Sale?> ReadAsync(int id)
        {
            Sale? sale = await _dbSet
                .Include(SaleQueries.IncludeSaleItems())
                .Include(SaleQueries.IncludePostings())
                .OrderByDescending(SaleQueries.OrderBySaleDate())
                .SingleOrDefaultAsync(SaleQueries.GetSalesById(id));
            return sale;
        }

        public async Task<IEnumerable<Sale>> ReadAllIncludingCustomerAsync()
        {
            return await _dbSet.AsNoTracking()
                .Include(SaleQueries.IncludeCustomer())
                .Include(SaleQueries.IncludePostings())
                .Include(SaleQueries.IncludeSaleItems())
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> ReadAllFromCustomerAsync(int customerId)
        {
            return await _dbSet.AsNoTracking()
                .Include(SaleQueries.IncludeCustomer())
                .Include(SaleQueries.IncludeSaleItems())
                .Include(SaleQueries.IncludePostings())
                .Where(SaleQueries.GetSalesByCustomer(customerId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> ReadAllOpenFromCustomerAsync(int customerId)
        {
            return await _dbSet//.AsNoTracking()
                .Include(SaleQueries.IncludeCustomer())//.AsNoTracking()
                .Include(SaleQueries.IncludeSaleItems())//.AsNoTracking()
                .Include(SaleQueries.IncludePostings())//.AsNoTracking()
                .Where(SaleQueries.GetSalesByCustomer(customerId))
                .Where(SaleQueries.GetSalesBySituation(ESaleSituation.Open))
                .ToListAsync();
        }
    }
}
