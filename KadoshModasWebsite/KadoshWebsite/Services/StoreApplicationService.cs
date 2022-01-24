using KadoshDomain.Commands;
using KadoshDomain.Services.Interfaces;
using KadoshShared.Constants.ServicesMessages;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class StoreApplicationService : IStoreApplicationService
    {
        private readonly IStoreService _storeService;

        public StoreApplicationService(IStoreService storeService)
        {
            _storeService = storeService;
        }

        public async Task CreateStoreAsync(StoreViewModel store)
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

            await _storeService.CreateStoreAsync(command);
        }

        public async Task<IEnumerable<StoreViewModel>> GetAllStoresAsync()
        {
            var stores = await _storeService.GetAllStoresAsync();
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
            var store = await _storeService.GetStoreAsync(id);
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

        public async Task UpdateStoreAsync(StoreViewModel store)
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

            await _storeService.UpdateStoreAsync(command);
        }

        public async Task DeleteStoreAsync(int id)
        {
            DeleteStoreCommand command = new();
            command.Id = id;

            await _storeService.DeleteStoreAsync(command);
        }
    }
}
