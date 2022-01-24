using KadoshDomain.Commands;
using KadoshDomain.Entities;

namespace KadoshDomain.Services.Interfaces
{
    public interface IStoreService
    {
        Task CreateStoreAsync(CreateStoreCommand command);
        Task<IEnumerable<Store>> GetAllStoresAsync();
        Task<Store> GetStoreAsync(int id);
        Task UpdateStoreAsync(UpdateStoreCommand command);
        Task DeleteStoreAsync(DeleteStoreCommand command);
    }
}
