using KadoshDomain.Entities;
using KadoshShared.Repositories;

namespace KadoshDomain.Repositories
{
    public interface IInstallmentRepository : IRepository<Installment>
    {
        Task<IEnumerable<Installment>> ReadAllInstallmentsFromSaleAsync(int saleId);
    }
}
