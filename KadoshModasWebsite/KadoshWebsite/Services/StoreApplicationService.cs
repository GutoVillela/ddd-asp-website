using KadoshDomain.Commands.StoreCommands.CreateStore;
using KadoshDomain.Commands.StoreCommands.DeleteStore;
using KadoshDomain.Commands.StoreCommands.UpdateStore;
using KadoshDomain.Queries.StoreQueries.GetAllStores;
using KadoshDomain.Queries.StoreQueries.GetStoreById;
using KadoshShared.Commands;
using KadoshShared.ExtensionMethods;
using KadoshShared.Handlers;
using KadoshWebsite.Infrastructure;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class StoreApplicationService : IStoreApplicationService
    {
        private readonly ICommandHandler<CreateStoreCommand> _createStoreHandler;
        private readonly ICommandHandler<DeleteStoreCommand> _deleteStoreHandler;
        private readonly ICommandHandler<UpdateStoreCommand> _updateStoreHandler;

        private readonly IQueryHandler<GetAllStoresQuery, GetAllStoresQueryResult> _getAllStoresQueryHandler;
        private readonly IQueryHandler<GetStoreByIdQuery, GetStoreByIdQueryResult> _getStoreByIdQueryHandler;

        public StoreApplicationService(
            ICommandHandler<CreateStoreCommand> createStoreHandler,
            ICommandHandler<DeleteStoreCommand> deleteStoreHandler,
            ICommandHandler<UpdateStoreCommand> updateStoreHandler,
            IQueryHandler<GetAllStoresQuery, GetAllStoresQueryResult> getAllStoresQueryHandler,
            IQueryHandler<GetStoreByIdQuery, GetStoreByIdQueryResult> getStoreByIdQueryHandler
            )
        {
            _createStoreHandler = createStoreHandler;
            _deleteStoreHandler = deleteStoreHandler;
            _updateStoreHandler = updateStoreHandler;
            _getAllStoresQueryHandler = getAllStoresQueryHandler;
            _getStoreByIdQueryHandler = getStoreByIdQueryHandler;
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
            var result = await _getAllStoresQueryHandler.HandleAsync(new GetAllStoresQuery());

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            var storeViewModels = new List<StoreViewModel>();

            foreach(var store in result.Stores)
            {
                storeViewModels.Add(new StoreViewModel()
                {
                    Id = store.Id,
                    Name = store.Name,
                    Street = store.Street,
                    Number = store.Number,
                    Neighborhood = store.Neighborhood,
                    City = store.City,
                    State = store.State,
                    ZipCode = store.ZipCode,
                    Complement = store.Complement
                });
            }
            return storeViewModels;

        }

        public async Task<StoreViewModel> GetStoreAsync(int id)
        {
            GetStoreByIdQuery query = new();
            query.StoreId = id;

            var result = await _getStoreByIdQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            StoreViewModel viewModel = new()
            {
                Id = result.Store!.Id,
                Name = result.Store!.Name,
                Street = result.Store!.Street,
                Number = result.Store!.Number,
                Neighborhood = result.Store!.Neighborhood,
                City = result.Store!.City,
                State = result.Store!.State,
                ZipCode = result.Store!.ZipCode,
                Complement = result.Store!.Complement
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

        public async Task<PaginatedListViewModel<StoreViewModel>> GetAllStoresPaginatedAsync(int currentPage, int pageSize)
        {
            GetAllStoresQuery query = new();
            query.CurrentPage = currentPage;
            query.PageSize = pageSize;

            var result = await _getAllStoresQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            var storeViewModels = new List<StoreViewModel>();

            foreach (var store in result.Stores)
            {
                storeViewModels.Add(new StoreViewModel()
                {
                    Id = store.Id,
                    Name = store.Name,
                    Street = store.Street,
                    Number = store.Number,
                    Neighborhood = store.Neighborhood,
                    City = store.City,
                    State = store.State,
                    ZipCode = store.ZipCode,
                    Complement = store.Complement
                });
            }
            PaginatedListViewModel<StoreViewModel> paginatedList = new();
            paginatedList.CurrentPage = currentPage;
            paginatedList.PageSize = pageSize;
            paginatedList.TotalItemsCount = result.StoresCount;
            paginatedList.TotalPages = PaginationManager.CalculateTotalPages(result.StoresCount, pageSize);
            paginatedList.Items = storeViewModels;

            return paginatedList;
        }
    }
}
