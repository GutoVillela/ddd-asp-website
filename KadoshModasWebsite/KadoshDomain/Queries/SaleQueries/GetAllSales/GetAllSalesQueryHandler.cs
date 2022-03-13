using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.SaleQueries.DTOs;
using KadoshDomain.Repositories;

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
            var sales = await _saleRepository.ReadAllIncludingCustomerAsync();
            HashSet<SaleBaseDTO> salesDTO = new();

            foreach (var sale in sales)
            {
                salesDTO.Add(sale);
            }

            GetAllSalesQueryResult result = new()
            {
                Sales = salesDTO
            };
            return result;
        }
    }
}
