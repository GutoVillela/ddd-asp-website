using KadoshDomain.Entities;
using KadoshDomain.Queriables;
using KadoshDomain.Repositories;
using KadoshRepository.Persistence.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace KadoshRepository.Repositories
{
    public class InstallmentRepository : Repository<Installment>, IInstallmentRepository
    {
        public InstallmentRepository(StoreDataContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<IEnumerable<Installment>> ReadAllInstallmentsFromSaleAsync(int saleId)
        {
            return await _dbSet
                .Where(InstallmentQueriable.GetBySaleId(saleId))
                .ToListAsync();

        }
    }
}
