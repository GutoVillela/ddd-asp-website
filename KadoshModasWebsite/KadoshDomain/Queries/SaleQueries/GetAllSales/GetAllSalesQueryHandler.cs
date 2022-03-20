using KadoshDomain.Entities;
using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.SaleQueries.DTOs;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

namespace KadoshDomain.Queries.SaleQueries.GetAllSales
{
    public class GetAllSalesQueryHandler : QueryHandlerBase<GetAllSalesQuery, GetAllSalesQueryResult>
    {
        private readonly ISaleRepository _saleRepository;

        public GetAllSalesQueryHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public override async Task<GetAllSalesQueryResult> HandleAsync(GetAllSalesQuery command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_ALL_SALES_QUERY);
                return new GetAllSalesQueryResult(errors);
            }

            IEnumerable<Sale> sales;

            bool queryIsNotPaginated = command.PageSize == 0 || command.CurrentPage == 0;

            if (queryIsNotPaginated)
                sales = await _saleRepository.ReadAllAsync();
            else
                sales = await _saleRepository.ReadAllPagedAsync(command.CurrentPage, command.PageSize);

            HashSet<SaleBaseDTO> salesDTO = new();

            foreach (var sale in sales)
            {
                salesDTO.Add(sale);
            }

            GetAllSalesQueryResult result = new()
            {
                Sales = salesDTO
            };

            if (queryIsNotPaginated)
                result.SalesCount = salesDTO.Count;
            else
                result.SalesCount = await _saleRepository.CountAllAsync();

            return result;
        }
    }
}
