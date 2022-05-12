using Kadosh.LegacyRepository.DAL;
using KadoshDomain.Entities;
using KadoshDomain.LegacyEntities;
using KadoshShared.Repositories;

namespace Kadosh.LegacyRepository.Repositories
{
    public class ProductLegacyRepository : ILegacyRepository<Product, ProductLegacy>
    {
        public async Task<IEnumerable<ProductLegacy>> ReadAllAsync(string connectionString)
        {
            ProductDAO productDAO = new(new Connection(connectionString));

            var productsFromLegacy = await productDAO.ReadAllAsync();
            return productsFromLegacy;
        }
    }
}
