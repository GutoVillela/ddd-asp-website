using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Handlers;
using KadoshDomain.Repositories;
using KadoshDomain.Services.Interfaces;
using KadoshShared.Commands;
using KadoshShared.Repositories;

namespace KadoshDomain.Services
{
    public class StoreService : IStoreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStoreRepository _storeRepository;

        public StoreService(IUnitOfWork unitOfWork, IStoreRepository storeRepository)
        {
            _unitOfWork = unitOfWork;
            _storeRepository = storeRepository;
        }

        public async Task<ICommandResult> CreateStoreAsync(CreateStoreCommand command)
        {
            StoreHandler storeHandler = new(_unitOfWork, _storeRepository);
            return await storeHandler.HandleAsync(command);
        }

        public async Task<IEnumerable<Store>> GetAllStoresAsync()
        {
            return await _storeRepository.ReadAllAsync();
        }

        public async Task<Store> GetStoreAsync(int id)
        {
            return await _storeRepository.ReadAsync(id);
        }

        public async Task<ICommandResult> UpdateStoreAsync(UpdateStoreCommand command)
        {
            StoreHandler storeHandler = new(_unitOfWork, _storeRepository);
            return await storeHandler.HandleAsync(command);
        }

        public async Task<ICommandResult> DeleteStoreAsync(DeleteStoreCommand command)
        {
            StoreHandler storeHandler = new(_unitOfWork, _storeRepository);
            return await storeHandler.HandleAsync(command);
            
        }
    }
}
