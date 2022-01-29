using KadoshShared.Commands;
using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface ICustomerApplicationService
    {
        Task<ICommandResult> CreateCustomerAsync(CustomerViewModel Customer);
        Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync();
        Task<CustomerViewModel> GetCustomerAsync(int id);
        Task<ICommandResult> UpdateCustomerAsync(CustomerViewModel Customer);
        Task<ICommandResult> DeleteCustomerAsync(int id);
    }
}
