using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.Queriables;
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
                .Include(SaleQueriable.IncludeSaleItems())
                .Include(SaleQueriable.IncludePostings())
                .OrderByDescending(SaleQueriable.OrderBySaleDate())
                .ToListAsync();
        }

        public override async Task<Sale?> ReadAsync(int id)
        {
            Sale? sale = await _dbSet
                .Include(SaleQueriable.IncludeSaleItems())
                .Include(SaleQueriable.IncludePostings())
                .OrderByDescending(SaleQueriable.OrderBySaleDate())
                .SingleOrDefaultAsync(SaleQueriable.GetSalesById(id));
            return sale;
        }

        public async Task<IEnumerable<Sale>> ReadAllIncludingCustomerAsync()
        {
            return await _dbSet.AsNoTracking()
                .Include(SaleQueriable.IncludeCustomer())
                .Include(SaleQueriable.IncludePostings())
                .Include(SaleQueriable.IncludeSaleItems())
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> ReadAllFromCustomerAsync(int customerId)
        {
            return await _dbSet.AsNoTracking()
                .Include(SaleQueriable.IncludeCustomer())
                .Include(SaleQueriable.IncludeSaleItems())
                .Include(SaleQueriable.IncludePostings())
                .Where(SaleQueriable.GetSalesByCustomer(customerId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> ReadAllOpenFromCustomerAsync(int customerId)
        {
            return await _dbSet
                .Include(SaleQueriable.IncludeCustomer())
                .Include(SaleQueriable.IncludeSaleItems())
                .Include(SaleQueriable.IncludePostings())
                .Where(SaleQueriable.GetSalesByCustomer(customerId))
                .Where(SaleQueriable.GetSalesBySituation(ESaleSituation.Open))
                .ToListAsync();
        }
    }
}
