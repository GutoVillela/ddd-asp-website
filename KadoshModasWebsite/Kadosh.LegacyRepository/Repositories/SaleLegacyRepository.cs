using Kadosh.LegacyRepository.DAL;
using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.LegacyEntities;
using KadoshShared.Repositories;

namespace Kadosh.LegacyRepository.Repositories
{
    public class SaleLegacyRepository : ILegacyRepository<Sale, SaleLegacy>
    {
        public async Task<IEnumerable<SaleLegacy>> ReadAllAsync(string connectionString)
        {
            SaleDAO saleDAO = new(new Connection(connectionString));

            var salesFromLegacy = await saleDAO.ReadAllAsync();

            foreach(var sale in salesFromLegacy)
            {
                // Get sale item
                var saleItems = await new SaleItemDAO(new Connection(connectionString)).ReadAllFromSaleLegacyAsync(sale.Id);
                sale.SaleItems = saleItems;

                // Get sale installments
                if(sale.SaleType == ESaleLegacyType.InInstallments)
                    sale.SaleInstallments = await new InstallmentDAO(new Connection(connectionString)).ReadAllFromSaleLegacyAsync(sale.Id);
            }

            return salesFromLegacy;
        }
    }
}