using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshRepository.Persistence.DataContexts;

namespace KadoshRepository.Repositories
{
    public class SaleInCashRepository : Repository<SaleInCash>, ISaleInCashRepository
    {
        public SaleInCashRepository(StoreDataContext dbContext) : base(dbContext)
        {
            
        }
    }
}
