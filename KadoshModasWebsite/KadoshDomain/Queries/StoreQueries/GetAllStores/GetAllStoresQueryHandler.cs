using KadoshDomain.Entities;
using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.StoreQueries.DTOs;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

namespace KadoshDomain.Queries.StoreQueries.GetAllStores
{
    public class GetAllStoresQueryHandler : QueryHandlerBase<GetAllStoresQuery, GetAllStoresQueryResult>
    {
        private readonly IStoreRepository _storeRepository;

        public GetAllStoresQueryHandler(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public override async Task<GetAllStoresQueryResult> HandleAsync(GetAllStoresQuery command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_ALL_STORES_QUERY);
                return new GetAllStoresQueryResult(errors);
            }

            IEnumerable<Store> stores;

            if (command.PageSize == 0 || command.CurrentPage == 0)
                stores = await _storeRepository.ReadAllAsync();
            else
                stores = await _storeRepository.ReadAllPagedAsync(command.CurrentPage, command.PageSize);

            HashSet<StoreDTO> storesDTO = new();

            foreach (var store in stores)
            {
                storesDTO.Add(store);
            }

            GetAllStoresQueryResult result = new()
            {
                Stores = storesDTO
            };
            result.StoresCount = await _storeRepository.CountAllAsync();
            return result;
        }
    }
}
