using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface ICustomerPostingApplicationService
    {
        Task<IEnumerable<CustomerPostingViewModel>> GetAllPostingsFromCustomerAsync(int customerId);
    }
}
