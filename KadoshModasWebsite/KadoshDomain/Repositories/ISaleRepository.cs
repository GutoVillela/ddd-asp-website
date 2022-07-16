using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshShared.Repositories;

namespace KadoshDomain.Repositories
{
    public interface ISaleRepository : IRepository<Sale>
    {
        Task<IEnumerable<Sale>> ReadAllIncludingCustomerAsync();
        Task<IEnumerable<Sale>> ReadAllFromCustomerAsync(int customerId);
        Task<IEnumerable<Sale>> ReadAllFromCustomerPaginatedAsync(int customerId, int currentPage, int pageSize);
        Task<IEnumerable<Sale>> ReadAllOpenFromCustomerAsync(int customerId);
        Task<IEnumerable<Sale>> ReadAllFromDateAsync(DateTime startDateUtc, DateTime endDateUtc);
        Task<IEnumerable<Sale>> ReadAllFromSituationAsync(ESaleSituation saleSituation);
        Task<IEnumerable<Sale>> ReadAllFromSituationPaginatedAsync(ESaleSituation saleSituation, int currentPage, int pageSize);
        Task<int> CountAllFromCustomerAsync(int customerId);
        Task<int> CountAllFromSituationAsync(ESaleSituation saleSituation);
    }
}