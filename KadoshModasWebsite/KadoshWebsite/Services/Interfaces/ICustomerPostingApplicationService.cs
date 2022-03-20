using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface ICustomerPostingApplicationService
    {
        Task<PaginatedListViewModel<CustomerPostingViewModel>> GetAllPostingsFromCustomerPaginatedAsync(int customerId, int currentPage, int pageSize);
    }
}
