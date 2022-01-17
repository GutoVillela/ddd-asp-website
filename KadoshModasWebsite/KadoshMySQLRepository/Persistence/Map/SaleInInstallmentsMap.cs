using KadoshDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KadoshRepository.Persistence.Map
{
    internal class SaleInInstallmentsMap : IEntityTypeConfiguration<SaleInInstallments>
    {
        public void Configure(EntityTypeBuilder<SaleInInstallments> builder)
        {
            builder.HasBaseType(typeof(Sale));
            builder.Property(x => x.InterestOnTheTotalSaleInPercentage).IsRequired();
            builder.HasMany(x => x.Installments).WithOne(x => x.Sale).HasForeignKey(x => x.SaleId).IsRequired();
        }
    }
}
