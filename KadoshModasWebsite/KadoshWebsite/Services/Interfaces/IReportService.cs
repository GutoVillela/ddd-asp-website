namespace KadoshWebsite.Services.Interfaces
{
    public interface IReportService
    {
        Task<int> GetWeekSellsAsync(TimeZoneInfo reportTimeZone); 
    }
}
