using KadoshDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KadoshRepository.Persistence.Map
{
    internal class SaleOnCreditMap : IEntityTypeConfiguration<SaleOnCredit>
    {
        public void Configure(EntityTypeBuilder<SaleOnCredit> builder)
        {
            builder.HasBaseType(typeof(Sale));
        }
    }
}
