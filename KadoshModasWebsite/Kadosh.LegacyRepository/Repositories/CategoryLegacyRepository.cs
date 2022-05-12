using Kadosh.LegacyRepository.DAL;
using KadoshDomain.Entities;
using KadoshDomain.LegacyEntities;
using KadoshShared.Repositories;

namespace Kadosh.LegacyRepository.Repositories
{
    public class CategoryLegacyRepository : ILegacyRepository<Category, CategoryLegacy>
    {
        public async Task<IEnumerable<CategoryLegacy>> ReadAllAsync(string connectionString)
        {
            CategoryDAO categoryDAO = new(new Connection(connectionString));

            var categoriesFromLegacy = await categoryDAO.ReadAllAsync();
            return categoriesFromLegacy;
        }
    }
}
