using KadoshShared.Commands;
using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface ICustomerApplicationService
    {
        Task<ICommandResult> CreateCustomerAsync(CustomerViewModel customer);
        Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync();
        Task<PaginatedListViewModel<CustomerViewModel>> GetAllCustomersPaginatedAsync(int currentPage, int pageSize);
        Task<PaginatedListViewModel<CustomerViewModel>> GetAllCustomersByNamePaginatedAsync(string customerName, int currentPage, int pageSize);
        Task<CustomerViewModel> GetCustomerAsync(int id);
        Task<ICommandResult> UpdateCustomerAsync(CustomerViewModel customer);
        Task<ICommandResult> DeleteCustomerAsync(int id);
        Task<decimal> GetCustomerTotalDebtAsync(int customerId);
        Task<ICommandResult> InformCustomerPaymentAsync(int customerId,decimal amountToInform);
    }
}
