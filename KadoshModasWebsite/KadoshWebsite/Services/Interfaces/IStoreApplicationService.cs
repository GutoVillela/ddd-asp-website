using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface IStoreApplicationService
    {
        Task CreateStoreAsync(StoreViewModel store);
        Task<IEnumerable<StoreViewModel>> GetAllStoresAsync();
        Task<StoreViewModel> GetStoreAsync(int id);
        Task UpdateStoreAsync(StoreViewModel store);
        Task DeleteStoreAsync(int id);
    }
}
