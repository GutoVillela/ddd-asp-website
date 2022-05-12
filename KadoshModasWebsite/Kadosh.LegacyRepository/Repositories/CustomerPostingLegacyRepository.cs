using Kadosh.LegacyRepository.DAL;
using KadoshDomain.Entities;
using KadoshDomain.LegacyEntities;
using KadoshShared.Repositories;

namespace Kadosh.LegacyRepository.Repositories
{
    public class CustomerPostingLegacyRepository : ILegacyRepository<CustomerPosting, CustomerPostingLegacy>
    {
        public async Task<IEnumerable<CustomerPostingLegacy>> ReadAllAsync(string connectionString)
        {
            CustomerPostingDAO customerPostingDAO = new(new Connection(connectionString));

            var postingsFromLegacy = await customerPostingDAO.ReadAllAsync();
            return postingsFromLegacy;
        }
    }
}