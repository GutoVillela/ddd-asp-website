using Kadosh.LegacyRepository.DAL;
using KadoshDomain.Entities;
using KadoshDomain.LegacyEntities;
using KadoshShared.Repositories;

namespace Kadosh.LegacyRepository.Repositories
{
    public class CustomerLegacyRepository : ILegacyRepository<Customer, CustomerLegacy>
    {
        public async Task<IEnumerable<CustomerLegacy>> ReadAllAsync(string connectionString)
        {
            CustomerDAO customerDAO = new(new Connection(connectionString));

            var customersFromLegacy = await customerDAO.ReadAllAsync();
            return customersFromLegacy;
        }
    }
}