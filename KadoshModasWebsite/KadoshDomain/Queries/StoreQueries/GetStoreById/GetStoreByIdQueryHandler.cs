using KadoshDomain.Queries.Base;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Constants.ServicesMessages;

namespace KadoshDomain.Queries.StoreQueries.GetStoreById
{
    public class GetStoreByIdQueryHandler : QueryHandlerBase<GetStoreByIdQuery, GetStoreByIdQueryResult>
    {
        private readonly IStoreRepository _storeRepository;

        public GetStoreByIdQueryHandler(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public override async Task<GetStoreByIdQueryResult> HandleAsync(GetStoreByIdQuery command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_STORE_BY_ID_QUERY_RESULT);
                return new GetStoreByIdQueryResult(errors);
            }

            var store = await _storeRepository.ReadAsync(command.StoreId!.Value);

            if (store is null)
            {
                AddNotification(nameof(store), StoreServiceMessages.ERROR_STORE_ID_NOT_FOUND);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_BRAND_NOT_FOUND);
                return new GetStoreByIdQueryResult(errors);
            }

            GetStoreByIdQueryResult brandViewModel = new()
            {
                Store = store
            };

            return brandViewModel;
        }
    }
}
