using KadoshDomain.Queries.CustomerQueries.GetAllDelinquentCustomers;
using KadoshDomain.Queries.SaleQueries.GetAllOpenSales;
using KadoshDomain.Queries.SaleQueries.GetSalesOfTheWeek;
using KadoshShared.ExtensionMethods;
using KadoshShared.Handlers;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class ReportService : IReportService
    {
        private readonly IQueryHandler<GetSalesOfTheWeekQuery, GetSalesOfTheWeekQueryResult> _getSalesOfTheWeekQueryHandler;
        private readonly IQueryHandler<GetAllDelinquentCustomersQuery, GetAllDelinquentCustomersQueryResult> _getAllDelinquentCustomersQueryHandler;
        private readonly IQueryHandler<GetAllOpenSalesQuery, GetAllOpenSalesQueryResult> _getAllOpenSalesQueryHandler;

        public ReportService(
            IQueryHandler<GetSalesOfTheWeekQuery, GetSalesOfTheWeekQueryResult> getSalesOfTheWeekQueryHandler,
            IQueryHandler<GetAllDelinquentCustomersQuery, GetAllDelinquentCustomersQueryResult> getAllDelinquentCustomersQueryHandler,
            IQueryHandler<GetAllOpenSalesQuery, GetAllOpenSalesQueryResult> getAllOpenSalesQueryHandler
            )
        {
            _getSalesOfTheWeekQueryHandler = getSalesOfTheWeekQueryHandler;
            _getAllDelinquentCustomersQueryHandler = getAllDelinquentCustomersQueryHandler;
            _getAllOpenSalesQueryHandler = getAllOpenSalesQueryHandler;
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
    }
}
