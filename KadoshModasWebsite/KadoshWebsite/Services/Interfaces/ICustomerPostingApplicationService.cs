using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface ICustomerPostingApplicationService
    {
        Task<PaginatedListViewModel<CustomerPostingViewModel>> GetAllPostingsFromCustomerPaginatedAsync(int customerId, int currentPage, int pageSize);
        Task<PaginatedListViewModel<CustomerPostingViewModel>> GetAllPostingsFromSalePaginatedAsync(int saleId, int currentPage, int pageSize);
    }
}
