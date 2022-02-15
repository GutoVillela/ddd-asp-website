using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshRepository.Persistence.DataContexts;

namespace KadoshRepository.Repositories
{
    public class SaleOnCreditRepository : Repository<SaleOnCredit>, ISaleOnCreditRepository
    {
        public SaleOnCreditRepository(StoreDataContext dbContext) : base(dbContext)
        {
            
        }
    }
}
