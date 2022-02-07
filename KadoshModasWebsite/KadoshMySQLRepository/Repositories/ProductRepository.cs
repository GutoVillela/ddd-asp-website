using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshRepository.Persistence.DataContexts;

namespace KadoshRepository.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(StoreDataContext dbContext) : base(dbContext)
        {
            
        }
    }
}
