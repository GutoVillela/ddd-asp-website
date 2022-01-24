using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Handlers;
using KadoshDomain.Repositories;
using KadoshDomain.Services.Interfaces;

namespace KadoshDomain.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;

        public StoreService(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public async Task CreateStoreAsync(CreateStoreCommand command)
        {
            StoreHandler storeHandler = new(_storeRepository);
            await storeHandler.HandleAsync(command);

            if (!storeHandler.IsValid)
                throw new ApplicationException(storeHandler.Notifications.ToString());
        }

        public async Task<IEnumerable<Store>> GetAllStoresAsync()
        {
            return await _storeRepository.ReadAllAsync();
        }

        public async Task<Store> GetStoreAsync(int id)
        {
            return await _storeRepository.ReadAsync(id);
        }

        public async Task UpdateStoreAsync(UpdateStoreCommand command)
        {
            StoreHandler storeHandler = new(_storeRepository);
            await storeHandler.HandleAsync(command);

            if (!storeHandler.IsValid)
                throw new ApplicationException(storeHandler.Notifications.ToString());
        }

        public async Task DeleteStoreAsync(DeleteStoreCommand command)
        {
            StoreHandler storeHandler = new(_storeRepository);
            await storeHandler.HandleAsync(command);

            if (!storeHandler.IsValid)
                throw new ApplicationException(storeHandler.Notifications.ToString());
        }
    }
}
