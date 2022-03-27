using KadoshDomain.Queries.Base;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Constants.QueriesMessages;

namespace KadoshDomain.Queries.SaleQueries.GetSaleById
{
    public class GetSaleByIdQueryHandler : QueryHandlerBase<GetSaleByIdQuery, GetSaleByIdQueryResult>
    {
        private readonly ISaleRepository _saleRepository;

        public GetSaleByIdQueryHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public override async Task<GetSaleByIdQueryResult> HandleAsync(GetSaleByIdQuery command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_SALE_BY_ID_QUERY);
                return new GetSaleByIdQueryResult(errors);
            }

            var sale = await _saleRepository.ReadAsync(command.SaleId!.Value);

            if (sale is null)
            {
                AddNotification(nameof(sale), SaleQueriesMessages.ERROR_SALE_NOT_FOUND);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_SALE_NOT_FOUND);
                return new GetSaleByIdQueryResult(errors);
            }

            GetSaleByIdQueryResult result = new()
            {
                Sale = sale
            };

            return result;
        }
    }
}
