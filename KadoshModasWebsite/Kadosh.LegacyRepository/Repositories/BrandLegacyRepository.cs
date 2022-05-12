using Kadosh.LegacyRepository.DAL;
using KadoshDomain.LegacyEntities;
using KadoshDomain.Entities;
using KadoshShared.Repositories;

namespace Kadosh.LegacyRepository.Repositories
{
    public class BrandLegacyRepository : ILegacyRepository<Brand, BrandLegacy>
    {

        public async Task<IEnumerable<BrandLegacy>> ReadAllAsync(string connectionString)
        {
            BrandDAO brandDAO = new(new Connection(connectionString));

            var brandsFromLegacy = await brandDAO.ReadAllAsync();
            return brandsFromLegacy;
        }
    }
}
