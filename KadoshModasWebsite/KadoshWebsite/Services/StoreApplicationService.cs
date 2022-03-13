using KadoshDomain.Commands.StoreCommands.CreateStore;
using KadoshDomain.Commands.StoreCommands.DeleteStore;
using KadoshDomain.Commands.StoreCommands.UpdateStore;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.ServicesMessages;
using KadoshShared.Handlers;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class StoreApplicationService : IStoreApplicationService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly ICommandHandler<CreateStoreCommand> _createStoreHandler;
        private readonly ICommandHandler<DeleteStoreCommand> _deleteStoreHandler;
        private readonly ICommandHandler<UpdateStoreCommand> _updateStoreHandler;

        public StoreApplicationService(
            IStoreRepository storeRepository,
            ICommandHandler<CreateStoreCommand> createStoreHandler,
            ICommandHandler<DeleteStoreCommand> deleteStoreHandler,
            ICommandHandler<UpdateStoreCommand> updateStoreHandler
            )
        {
            _storeRepository = storeRepository;
            _createStoreHandler = createStoreHandler;
            _deleteStoreHandler = deleteStoreHandler;
            _updateStoreHandler = updateStoreHandler;
        }

        public async Task<ICommandResult> CreateStoreAsync(StoreViewModel store)
        {
            CreateStoreCommand command = new();
            command.Name = store.Name;
            command.AddressStreet = store.Street;
            command.AddressNumber = store.Number;
            command.AddressNeighborhood = store.Neighborhood;
            command.AddressCity = store.City;
            command.AddressState = store.State;
            command.AddressZipCode = store.ZipCode;
            command.AddressComplement  = store.Complement;

            return await _createStoreHandler.HandleAsync(command);
        }

        public async Task<IEnumerable<StoreViewModel>> GetAllStoresAsync()
        {
            var stores = await _storeRepository.ReadAllAsync();
            var storeViewModels = new List<StoreViewModel>();

            foreach(var store in stores)
            {
                storeViewModels.Add(new StoreViewModel()
                {
                    Id = store.Id,
                    Name = store.Name,
                    Street = store.Address?.Street,
                    Number = store.Address?.Number,
                    Neighborhood = store.Address?.Neighborhood,
                    City = store.Address?.City,
                    State = store.Address?.State,
                    ZipCode = store.Address?.ZipCode,
                    Complement = store.Address?.Complement
                });
            }
            return storeViewModels;

        }

        public async Task<StoreViewModel> GetStoreAsync(int id)
        {
            var store = await _storeRepository.ReadAsync(id);
            if (store == null)
                throw new ApplicationException(StoreServiceMessages.ERROR_STORE_ID_NOT_FOUND);

            StoreViewModel viewModel = new()
            {
                Id = store.Id,
                Name = store.Name,
                Street = store.Address?.Street,
                Number = store.Address?.Number,
                Neighborhood = store.Address?.Neighborhood,
                City = store.Address?.City,
                State = store.Address?.State,
                ZipCode = store.Address?.ZipCode,
                Complement = store.Address?.Complement
            };

            return viewModel;
        }

        public async Task<ICommandResult> UpdateStoreAsync(StoreViewModel store)
        {
            UpdateStoreCommand command = new();
            command.Id = store.Id;
            command.Name = store.Name;
            command.AddressStreet = store.Street;
            command.AddressNumber = store.Number;
            command.AddressNeighborhood = store.Neighborhood;
            command.AddressCity = store.City;
            command.AddressState = store.State;
            command.AddressZipCode = store.ZipCode;
            command.AddressComplement = store.Complement;

            return await _updateStoreHandler.HandleAsync(command);

        }

        public async Task<ICommandResult> DeleteStoreAsync(int id)
        {
            DeleteStoreCommand command = new();
            command.Id = id;

            return await _deleteStoreHandler.HandleAsync(command);
        }
    }
}
