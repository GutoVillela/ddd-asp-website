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
        Task<PaginatedListViewModel<SaleViewModel>> GetAllSalesIncludingProductsByCustomerPaginatedAsync(int customerId, int currentPage, int pageSize);
        Task<ICommandResult> PayOffSaleAsync(int saleId);
        Task<SaleViewModel> GetSaleAsync(int saleId);
        Task<ICommandResult> InformPaymentAsync(int saleId, decimal amountToInform);
        Task<ICommandResult> PayOffInstallmentAsync(int saleId, int installmentId);
        Task<ICommandResult> CancelSaleAsync(int saleId);
        Task<ICommandResult> CancelSaleItemAsync(int saleId, int productId, int amountToCancel);
    }
}
