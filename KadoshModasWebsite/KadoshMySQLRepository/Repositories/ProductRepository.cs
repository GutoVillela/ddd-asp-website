using KadoshDomain.Entities;
using KadoshDomain.Queriables;
using KadoshDomain.Repositories;
using KadoshRepository.Persistence.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace KadoshRepository.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(StoreDataContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<int> CountAllByNameAsync(string productName)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(ProductQueriable.GetProductByName(productName))
                .CountAsync();
        }

        public async Task<IEnumerable<Product>> ReadAllByNameAsync(string productName)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(ProductQueriable.GetProductByName(productName))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> ReadAllByNamePagedAsync(string productName, int currentPage, int pageSize)
        {
            int amountToTake = (currentPage - 1) * pageSize;
            return await _dbSet
                .AsNoTracking()
                .Where(ProductQueriable.GetProductByName(productName))
                .Skip(amountToTake)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Product?> ReadByBarCodeAsync(string barCode)
        {
            Product? entity = await _dbSet.SingleOrDefaultAsync(ProductQueriable.GetByBarCode(barCode));
            return entity;
        }
    }
}
