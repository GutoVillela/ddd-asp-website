using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.SaleQueries.DTOs;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

namespace KadoshDomain.Queries.SaleQueries.GetSalesByDate
{
    public class GetSalesByDateQueryHandler : QueryHandlerBase<GetSalesByDateQuery, GetSalesByDateQueryResult>
    {
        private readonly ISaleRepository _saleRepository;

        public GetSalesByDateQueryHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public override async Task<GetSalesByDateQueryResult> HandleAsync(GetSalesByDateQuery query)
        {
            // Fail Fast Validations
            query.Validate();
            if (!query.IsValid)
            {
                AddNotifications(query);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_SALES_BY_DATE_QUERY);
                return new GetSalesByDateQueryResult(errors);
            }

            DateTime utcStartDate = TimeZoneInfo.ConvertTimeToUtc(query.StartDate!.Value, query.LocalTimeZone!);
            DateTime utcEndDate = TimeZoneInfo.ConvertTimeToUtc(query.EndDate!.Value, query.LocalTimeZone!);

            var sales = await _saleRepository.ReadAllFromDateAsync(utcStartDate, utcEndDate);

            HashSet<SaleBaseDTO> salesDTO = new();
            decimal totalAmountFromSales = 0;
            decimal totalAmountToPayFromSales = 0;
            decimal totalAmountPaidFromSales = 0;

            foreach (var sale in sales)
            {
                salesDTO.Add(sale);
                totalAmountFromSales += sale.Total;
                totalAmountToPayFromSales += sale.TotalToPay;
                totalAmountPaidFromSales += sale.TotalPaid;
            }

            GetSalesByDateQueryResult result = new()
            {
                Sales = salesDTO,
                SalesCount = salesDTO.Count,
                TotalAmountFromSales = totalAmountFromSales,
                TotalAmountToPayFromSales = totalAmountToPayFromSales,
                TotalAmountPaidFromSales = totalAmountPaidFromSales
            };

            return result;
        }
    }
}
