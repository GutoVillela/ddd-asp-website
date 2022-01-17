using KadoshDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KadoshRepository.Persistence.Map
{
    internal class SaleInCashMap : IEntityTypeConfiguration<SaleInCash>
    {
        public void Configure(EntityTypeBuilder<SaleInCash> builder)
        {
            builder.HasBaseType(typeof(Sale));
        }
    }
}
