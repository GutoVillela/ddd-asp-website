using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.SaleQueries.DTOs;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

namespace KadoshDomain.Queries.SaleQueries.GetAllOpenSales
{
    public class GetAllOpenSalesQueryHandler : QueryHandlerBase<GetAllOpenSalesQuery, GetAllOpenSalesQueryResult>
    {
        private readonly ISaleRepository _saleRepository;

        public GetAllOpenSalesQueryHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public override async Task<GetAllOpenSalesQueryResult> HandleAsync(GetAllOpenSalesQuery query)
        {
            // Fail Fast Validations
            query.Validate();
            if (!query.IsValid)
            {
                AddNotifications(query);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_ALL_OPEN_SALES_QUERY);
                return new GetAllOpenSalesQueryResult(errors);
            }

            IEnumerable<Sale> sales;

            bool queryIsNotPaginated = query.PageSize == 0 || query.CurrentPage == 0;

            if (queryIsNotPaginated)
                sales = await _saleRepository.ReadAllFromSituationAsync(ESaleSituation.Open);
            else
                sales = await _saleRepository.ReadAllFromSituationPaginatedAsync(ESaleSituation.Open, query.CurrentPage, query.PageSize);

            HashSet<SaleBaseDTO> salesDTO = new();

            foreach (var sale in sales)
            {
                salesDTO.Add(sale);
            }

            GetAllOpenSalesQueryResult result = new()
            {
                OpenSales = salesDTO
            };

            if (queryIsNotPaginated)
                result.OpenSalesCount = salesDTO.Count;
            else
                result.OpenSalesCount = await _saleRepository.CountAllAsync();

            return result;
        }
    }
}
