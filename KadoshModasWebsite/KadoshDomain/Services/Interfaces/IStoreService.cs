using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshShared.Commands;

namespace KadoshDomain.Services.Interfaces
{
    public interface IStoreService
    {
        Task<ICommandResult> CreateStoreAsync(CreateStoreCommand command);
        Task<IEnumerable<Store>> GetAllStoresAsync();
        Task<Store> GetStoreAsync(int id);
        Task<ICommandResult> UpdateStoreAsync(UpdateStoreCommand command);
        Task<ICommandResult> DeleteStoreAsync(DeleteStoreCommand command);
    }
}
