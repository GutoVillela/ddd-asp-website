using KadoshDomain.Entities;
using KadoshDomain.ValueObjects;
using KadoshShared.Repositories;

namespace KadoshDomain.Repositories
{
    public interface IStoreRepository : IRepository<Store>
    {
        Task<bool> AddressExists(Address address);
        Task<bool> AddressExistsExceptForGivenStore(Address address, int storeToDisconsider);
    }
}
