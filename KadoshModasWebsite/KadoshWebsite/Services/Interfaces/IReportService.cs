using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface IReportService
    {
        Task<int> GetWeekSellsCountAsync(TimeZoneInfo reportTimeZone);
        Task<int> GetDelinquentCustomersCountAsync(int intervalSinceLastPaymentInDays);
        Task<decimal> GetTotalToReceiveFromSalesAsync();

        Task<ChartReportModel> GetAllSalesFromLast30DaysAsync(TimeZoneInfo reportTimeZone);
    }
}
