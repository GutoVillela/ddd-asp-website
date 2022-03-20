using KadoshShared.Commands;
using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface ISaleApplicationService
    {
        Task<ICommandResult> CreateSaleAsync(SaleViewModel sale);
        Task<IEnumerable<SaleViewModel>> GetAllSalesAsync();
        Task<PaginatedListViewModel<SaleViewModel>> GetAllSalesPaginatedAsync(int currentPage, int pageSize);
        Task<IEnumerable<SaleViewModel>> GetAllSalesByCustomerAsync(int customerId);
        Task<PaginatedListViewModel<SaleViewModel>> GetAllSalesByCustomerPaginatedAsync(int customerId, int currentPage, int pageSize);
        Task<ICommandResult> PayOffSaleAsync(int saleId);
    }
}
