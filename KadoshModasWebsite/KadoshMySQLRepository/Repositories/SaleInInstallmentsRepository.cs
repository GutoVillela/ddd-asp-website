using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshRepository.Persistence.DataContexts;

namespace KadoshRepository.Repositories
{
    public class SaleInInstallmentsRepository : Repository<SaleInInstallments>, ISaleInInstallmentsRepository
    {
        public SaleInInstallmentsRepository(StoreDataContext dbContext) : base(dbContext)
        {
            
        }
    }
}
