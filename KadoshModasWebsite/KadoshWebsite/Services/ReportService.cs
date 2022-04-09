using KadoshDomain.Queries.CustomerQueries.GetAllDelinquentCustomers;
using KadoshDomain.Queries.SaleQueries.GetAllOpenSales;
using KadoshDomain.Queries.SaleQueries.GetSalesByDate;
using KadoshDomain.Queries.SaleQueries.GetSalesOfTheWeek;
using KadoshShared.ExtensionMethods;
using KadoshShared.Handlers;
using KadoshWebsite.Infrastructure;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class ReportService : IReportService
    {
        private readonly IQueryHandler<GetSalesOfTheWeekQuery, GetSalesOfTheWeekQueryResult> _getSalesOfTheWeekQueryHandler;
        private readonly IQueryHandler<GetAllDelinquentCustomersQuery, GetAllDelinquentCustomersQueryResult> _getAllDelinquentCustomersQueryHandler;
        private readonly IQueryHandler<GetAllOpenSalesQuery, GetAllOpenSalesQueryResult> _getAllOpenSalesQueryHandler;
        private readonly IQueryHandler<GetSalesByDateQuery, GetSalesByDateQueryResult> _getSalesByDateQueryHandler;

        public ReportService(
            IQueryHandler<GetSalesOfTheWeekQuery, GetSalesOfTheWeekQueryResult> getSalesOfTheWeekQueryHandler,
            IQueryHandler<GetAllDelinquentCustomersQuery, GetAllDelinquentCustomersQueryResult> getAllDelinquentCustomersQueryHandler,
            IQueryHandler<GetAllOpenSalesQuery, GetAllOpenSalesQueryResult> getAllOpenSalesQueryHandler,
            IQueryHandler<GetSalesByDateQuery, GetSalesByDateQueryResult> getSalesByDateQueryHandler
            )
        {
            _getSalesOfTheWeekQueryHandler = getSalesOfTheWeekQueryHandler;
            _getAllDelinquentCustomersQueryHandler = getAllDelinquentCustomersQueryHandler;
            _getAllOpenSalesQueryHandler = getAllOpenSalesQueryHandler;
            _getSalesByDateQueryHandler = getSalesByDateQueryHandler;
        }

        public async Task<int> GetWeekSellsCountAsync(TimeZoneInfo reportTimeZone)
        {
            GetSalesOfTheWeekQuery query = new();
            query.LocalTimeZone = reportTimeZone;

            var result = await _getSalesOfTheWeekQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            return result.SalesOfTheWeekCount;
        }

        public async Task<int> GetDelinquentCustomersCountAsync(int intervalSinceLastPaymentInDays)
        {
            GetAllDelinquentCustomersQuery query = new();
            query.IntervalSinceLastPaymentInDays = intervalSinceLastPaymentInDays;

            var result = await _getAllDelinquentCustomersQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            return result.DelinquentCustomersCount;
        }

        public async Task<decimal> GetTotalToReceiveFromSalesAsync()
        {
            GetAllOpenSalesQuery query = new();

            var result = await _getAllOpenSalesQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            decimal totalToReceive = 0;
            foreach(var openSale in result.OpenSales)
            {
                totalToReceive += openSale.TotalToPay;
            }

            return totalToReceive;
        }

        public async Task<ChartReportModel> GetAllSalesFromLast30DaysAsync(TimeZoneInfo reportTimeZone)
        {
            GetSalesByDateQuery query = new();
            query.LocalTimeZone = reportTimeZone;
            query.StartDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(-30), reportTimeZone);
            query.EndDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, reportTimeZone);

            var result = await _getSalesByDateQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            List<string> labels = new();
            List<string> salesInTheDay = new();//Data to display

            var salesGroupedByDate = result.Sales.GroupBy(i => TimeZoneInfo.ConvertTimeFromUtc(i.SaleDate!.Value.Date, reportTimeZone)).ToList();

            foreach (var saleGroup in salesGroupedByDate)
            {
                labels.Add(saleGroup.Key.ToString(FormatProviderManager.DateTimeFormat));
                salesInTheDay.Add(saleGroup.Count().ToString());
            }

            return new()
            {
                Labels = labels,
                Data = salesInTheDay
            };
        }
    }
}
