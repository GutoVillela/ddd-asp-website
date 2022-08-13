using KadoshDomain.Entities;
using KadoshShared.Repositories;

namespace KadoshDomain.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<IEnumerable<Customer>> ReadAllByNameAsync(string customerName, bool includeInactive = false);
        Task<IEnumerable<Customer>> ReadAllByNamePaginatedAsync(string customerName, int currentPage, int pageSize, bool includeInactive = false);
        Task<int> CountAllByNameAsync(string customerName, bool includeInactive = false);
        Task<Customer?> GetCustomerByUsernameAsync(string username);
        (string hash, byte[] salt, int iterations) GetPasswordHashed(string password);
        string GetPasswordHashed(string password, byte[] salt, int iterations);
        Task<bool> VerifyIfUsernameIsTakenAsync(string username);
    }
}
