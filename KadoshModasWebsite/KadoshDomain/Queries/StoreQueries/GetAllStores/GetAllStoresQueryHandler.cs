using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.StoreQueries.DTOs;
using KadoshDomain.Repositories;

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
            var stores = await _storeRepository.ReadAllAsync();
            HashSet<StoreDTO> storesDTO = new();

            foreach (var store in stores)
            {
                storesDTO.Add(store);
            }

            GetAllStoresQueryResult result = new()
            {
                Stores = storesDTO
            };
            return result;
        }
    }
}
