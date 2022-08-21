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

        public override async Task<IEnumerable<Sale>> ReadAllAsync(bool includeInactive = true)
        {
            // TODO: Review includeInactive param
            return await _dbSet
                .AsNoTracking()
                .Include(SaleQueriable.IncludeSaleItems())
                .Include(SaleQueriable.IncludePostings())
                .Include(SaleQueriable.IncludeCustomer())
                .Include(SaleQueriable.IncludeStore())
                .OrderByDescending(SaleQueriable.OrderBySaleDate())
                .ToListAsync();
        }


        public override async Task<IEnumerable<Sale>> ReadAllPagedAsync(int currentPage, int pageSize, bool includeInactive = true)
        {
            // TODO: Review includeInactive param
            int amountToTake = (currentPage - 1) * pageSize;
            return await _dbSet
                .AsNoTracking()
                .Include(SaleQueriable.IncludeSaleItems())
                .Include(SaleQueriable.IncludePostings())
                .Include(SaleQueriable.IncludeCustomer())
                .Include(SaleQueriable.IncludeStore())
                .OrderByDescending(SaleQueriable.OrderBySaleDate())
                .Skip(amountToTake)
                .Take(pageSize)
                .ToListAsync();
        }

        public override async Task<Sale?> ReadAsync(int id)
        {
            Sale? sale = await _dbSet
                .Include(SaleQueriable.IncludeSaleItems())
                .Include(SaleQueriable.IncludePostings())
                .Include(SaleQueriable.IncludeCustomer())
                .Include(SaleQueriable.IncludeStore())
                .OrderByDescending(SaleQueriable.OrderBySaleDate())
                .SingleOrDefaultAsync(SaleQueriable.GetById(id));
            return sale;
        }

        public async Task<IEnumerable<Sale>> ReadAllIncludingCustomerAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Include(SaleQueriable.IncludeCustomer())
                .Include(SaleQueriable.IncludePostings())
                .Include(SaleQueriable.IncludeSaleItems())
                .Include(SaleQueriable.IncludeStore())
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> ReadAllFromCustomerAsync(int customerId)
        {
            return await _dbSet
                //.AsNoTracking()
                .Include(SaleQueriable.IncludeCustomer())
                .Include(SaleQueriable.IncludeSaleItems())
                .Include(SaleQueriable.IncludePostings())
                .Include(SaleQueriable.IncludeStore())
                .Where(SaleQueriable.GetSalesByCustomer(customerId))
                .OrderByDescending(SaleQueriable.OrderBySaleDate())
                .ToListAsync();
        }

        // TODO Change method do fetch all from given sale situation not only open sales
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

        public async Task<IEnumerable<Sale>> ReadAllFromCustomerPaginatedAsync(int customerId, int currentPage, int pageSize)
        {
            int amountToTake = (currentPage - 1) * pageSize;
            return await _dbSet
                .AsNoTracking()
                .Include(SaleQueriable.IncludeCustomer())
                .Include(SaleQueriable.IncludeSaleItems())
                .Include(SaleQueriable.IncludePostings())
                .Include(SaleQueriable.IncludeStore())
                .Where(SaleQueriable.GetSalesByCustomer(customerId))
                .OrderByDescending(SaleQueriable.OrderBySaleDate())
                .Skip(amountToTake)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountAllFromCustomerAsync(int customerId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(SaleQueriable.GetSalesByCustomer(customerId))
                .CountAsync();
        }

        public async Task<IEnumerable<Sale>> ReadAllFromDateAsync(DateTime startDateUtc, DateTime endDateUtc)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(SaleQueriable.IncludeCustomer())
                .Include(SaleQueriable.IncludeSaleItems())
                .Include(SaleQueriable.IncludePostings())
                .Include(SaleQueriable.IncludeStore())
                .Where(SaleQueriable.GetSalesByDate(startDateUtc, endDateUtc))
                .OrderByDescending(SaleQueriable.OrderBySaleDate())
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> ReadAllFromSituationAsync(ESaleSituation saleSituation)
        {
            return await _dbSet
                .Include(SaleQueriable.IncludeCustomer())
                .Include(SaleQueriable.IncludeSaleItems())
                .Include(SaleQueriable.IncludePostings())
                .Where(SaleQueriable.GetSalesBySituation(saleSituation))
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> ReadAllFromSituationPaginatedAsync(ESaleSituation saleSituation, int currentPage, int pageSize)
        {
            int amountToTake = (currentPage - 1) * pageSize;
            return await _dbSet
                .Include(SaleQueriable.IncludeCustomer())
                .Include(SaleQueriable.IncludeSaleItems())
                .Include(SaleQueriable.IncludePostings())
                .Where(SaleQueriable.GetSalesBySituation(saleSituation))
                .Skip(amountToTake)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountAllFromSituationAsync(ESaleSituation saleSituation)
        {
            return await _dbSet
                .Where(SaleQueriable.GetSalesBySituation(saleSituation))
                .CountAsync();
        }
    }
}