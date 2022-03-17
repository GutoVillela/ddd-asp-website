using KadoshShared.Commands;
using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface IStoreApplicationService
    {
        Task<ICommandResult> CreateStoreAsync(StoreViewModel store);
        Task<IEnumerable<StoreViewModel>> GetAllStoresAsync();
        Task<PaginatedListViewModel<StoreViewModel>> GetAllStoresPaginatedAsync(int currentPage, int pageSize);
        Task<StoreViewModel> GetStoreAsync(int id);
        Task<ICommandResult> UpdateStoreAsync(StoreViewModel store);
        Task<ICommandResult> DeleteStoreAsync(int id);
    }
}
