using KadoshDomain.Queries.SaleQueries.GetSalesOfTheWeek;
using KadoshShared.ExtensionMethods;
using KadoshShared.Handlers;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class ReportService : IReportService
    {
        private readonly IQueryHandler<GetSalesOfTheWeekQuery, GetSalesOfTheWeekQueryResult> _getSalesOfTheWeekQueryHandler;

        public ReportService(IQueryHandler<GetSalesOfTheWeekQuery, GetSalesOfTheWeekQueryResult> getSalesOfTheWeekQueryHandler)
        {
            _getSalesOfTheWeekQueryHandler = getSalesOfTheWeekQueryHandler;
        }

        public async Task<int> GetWeekSellsAsync(TimeZoneInfo reportTimeZone)
        {
            GetSalesOfTheWeekQuery query = new();
            query.LocalTimeZone = reportTimeZone;

            var result = await _getSalesOfTheWeekQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());


            return result.SalesOfTheWeekCount;
        }
    }
}
