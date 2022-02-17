using KadoshDomain.Commands;
using KadoshDomain.Handlers;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.ServicesMessages;
using KadoshShared.Repositories;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class StoreApplicationService : IStoreApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStoreRepository _storeRepository;

        public StoreApplicationService(IUnitOfWork unitOfWork, IStoreRepository storeRepository)
        {
            _unitOfWork = unitOfWork;
            _storeRepository = storeRepository;
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

            StoreHandler storeHandler = new(_unitOfWork, _storeRepository);
            return await storeHandler.HandleAsync(command);
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

            StoreHandler storeHandler = new(_unitOfWork, _storeRepository);
            return await storeHandler.HandleAsync(command);

        }

        public async Task<ICommandResult> DeleteStoreAsync(int id)
        {
            DeleteStoreCommand command = new();
            command.Id = id;

            StoreHandler storeHandler = new(_unitOfWork, _storeRepository);
            return await storeHandler.HandleAsync(command);
        }
    }
}
