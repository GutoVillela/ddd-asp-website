using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshShared.Commands;

namespace KadoshDomain.Services.Interfaces
{
    public interface ISaleService
    {
        Task<ICommandResult> CreateSaleInCashAsync(CreateSaleInCashCommand command);
        Task<ICommandResult> CreateSaleInInstallmentsAsync(CreateSaleInInstallmentsCommand command);
        Task<ICommandResult> CreateSaleOnCreditAsync(CreateSaleOnCreditCommand command);
        Task<IEnumerable<Sale>> GetAllSalesIncludingCustomerAsync();
    }
}
