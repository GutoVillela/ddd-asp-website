using KadoshDomain.Entities;
using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.SaleQueries.DTOs;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

namespace KadoshDomain.Queries.SaleQueries.GetAllSalesByCustomerId
{
    public class GetAllSalesByCustomerIdQueryHandler : QueryHandlerBase<GetAllSalesByCustomerIdQuery, GetAllSalesByCustomerIdQueryResult>
    {
        private readonly ISaleRepository _saleRepository;

        public GetAllSalesByCustomerIdQueryHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public override async Task<GetAllSalesByCustomerIdQueryResult> HandleAsync(GetAllSalesByCustomerIdQuery command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_ALL_SALES_BY_CUSTOMER_ID_QUERY);
                return new GetAllSalesByCustomerIdQueryResult(errors);
            }

            IEnumerable<Sale> sales;

            bool queryIsNotPaginated = command.PageSize == 0 || command.CurrentPage == 0;

            if (queryIsNotPaginated)
                sales = await _saleRepository.ReadAllFromCustomerAsync(command.CustomerId!.Value);
            else
                sales = await _saleRepository.ReadAllFromCustomerPaginatedAsync(command.CustomerId!.Value, command.CurrentPage, command.PageSize);

            HashSet<SaleBaseDTO> salesDTO = new();

            foreach (var sale in sales)
            {
                salesDTO.Add(sale);
            }

            GetAllSalesByCustomerIdQueryResult result = new()
            {
                Sales = salesDTO
            };

            if (queryIsNotPaginated)
                result.SalesCount = salesDTO.Count;
            else
                result.SalesCount = await _saleRepository.CountAllFromCustomerAsync(command.CustomerId!.Value);

            return result;
        }
    }
}
