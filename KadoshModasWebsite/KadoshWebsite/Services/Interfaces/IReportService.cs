namespace KadoshWebsite.Services.Interfaces
{
    public interface IReportService
    {
        Task<int> GetWeekSellsCountAsync(TimeZoneInfo reportTimeZone);
        Task<int> GetDelinquentCustomersCountAsync(int intervalSinceLastPaymentInDays);
        Task<decimal> GetTotalToReceiveFromSalesAsync();
    }
}
