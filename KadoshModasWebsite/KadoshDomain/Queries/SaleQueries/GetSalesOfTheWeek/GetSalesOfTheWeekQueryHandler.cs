using KadoshDomain.Queries.Base;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;
using KadoshDomain.Util;
using KadoshDomain.Queries.SaleQueries.DTOs;

namespace KadoshDomain.Queries.SaleQueries.GetSalesOfTheWeek
{
    public class GetSalesOfTheWeekQueryHandler : QueryHandlerBase<GetSalesOfTheWeekQuery, GetSalesOfTheWeekQueryResult>
    {
        private readonly ISaleRepository _saleRepository;

        public GetSalesOfTheWeekQueryHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public override async Task<GetSalesOfTheWeekQueryResult> HandleAsync(GetSalesOfTheWeekQuery query)
        {
            // Fail Fast Validations
            query.Validate();
            if (!query.IsValid)
            {
                AddNotifications(query);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_SALES_OF_THE_WEEK_QUERY);
                return new GetSalesOfTheWeekQueryResult(errors);
            }

            DateTime localToday = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, query.LocalTimeZone!);
            (DateTime localStartDate, DateTime localEndDate) = DateTimeUtil.GetWeekRangeFromDate(localToday);
            DateTime utcStartDate = TimeZoneInfo.ConvertTimeToUtc(localStartDate, query.LocalTimeZone!);
            DateTime utcEndDate = TimeZoneInfo.ConvertTimeToUtc(localEndDate, query.LocalTimeZone!);

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

            GetSalesOfTheWeekQueryResult result = new()
            {
                SalesOfTheWeek = salesDTO,
                SalesOfTheWeekCount = salesDTO.Count,
                TotalAmountFromSalesOfTheWeek = totalAmountFromSales,
                TotalAmountToPayFromSalesOfTheWeek = totalAmountToPayFromSales,
                TotalAmountPaidFromSalesOfTheWeek = totalAmountPaidFromSales
            };

            return result;
        }
    }
}
