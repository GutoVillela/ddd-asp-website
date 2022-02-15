using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshRepository.Persistence.DataContexts;

namespace KadoshRepository.Repositories
{
    public class InstallmentRepository : Repository<Installment>, IInstallmentRepository
    {
        public InstallmentRepository(StoreDataContext dbContext) : base(dbContext)
        {
            
        }
    }
}
