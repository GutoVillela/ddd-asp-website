using KadoshShared.Commands;
using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface ICustomerApplicationService
    {
        Task<ICommandResult> CreateCustomerAsync(CustomerViewModel customer);
        Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync();
        Task<CustomerViewModel> GetCustomerAsync(int id);
        Task<ICommandResult> UpdateCustomerAsync(CustomerViewModel customer);
        Task<ICommandResult> DeleteCustomerAsync(int id);
    }
}
