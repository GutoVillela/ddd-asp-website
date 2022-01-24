using KadoshDomain.Entities;
using KadoshDomain.Queries;
using KadoshDomain.Repositories;
using KadoshDomain.ValueObjects;
using KadoshRepository.Persistence.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace KadoshRepository.Repositories
{
    public class StoreRepository : Repository<Store>, IStoreRepository
    {
        public StoreRepository(StoreDataContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddressExists(Address address)
        {
            return await _dbSet.AnyAsync(StoreQueries.GetStoreByAddress(address));
        }
        public async Task<bool> AddressExistsExceptForGivenStore(Address address, int storeToDisconsider)
        {
            return await _dbSet.AnyAsync(StoreQueries.GetStoreByAddressExceptWithGivenId(address, storeToDisconsider));
        }
    }
}
