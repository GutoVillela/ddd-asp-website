using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface ICustomerPostingApplicationService
    {
        Task<PaginatedCustomerPostingsViewModel> GetAllPostingsFromCustomerPaginatedAsync(int customerId, int currentPage, int pageSize);
        Task<PaginatedCustomerPostingsViewModel> GetAllPostingsFromSalePaginatedAsync(int saleId, int currentPage, int pageSize);
        Task<PaginatedCustomerPostingsViewModel> GetAllPostingsFromStoreAndDatePaginatedAsync(DateOnly date, TimeZoneInfo timeZone, int storeId, bool getTotal, int currentPage, int pageSize);
    }
}
