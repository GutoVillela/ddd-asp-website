using KadoshShared.Commands;
using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface ISaleApplicationService
    {
        Task<ICommandResult> CreateSaleAsync(SaleViewModel sale);
        Task<IEnumerable<SaleViewModel>> GetAllSalesAsync();
        Task<IEnumerable<SaleViewModel>> GetAllSalesByCustomerAsync(int customerId);
    }
}
