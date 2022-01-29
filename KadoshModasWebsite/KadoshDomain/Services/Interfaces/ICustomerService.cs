using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshShared.Commands;

namespace KadoshDomain.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<ICommandResult> CreateCustomerAsync(CreateCustomerCommand command);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerAsync(int id);
        Task<ICommandResult> UpdateCustomerAsync(UpdateCustomerCommand command);
        Task<ICommandResult> DeleteCustomerAsync(DeleteCustomerCommand command);
    }
}
