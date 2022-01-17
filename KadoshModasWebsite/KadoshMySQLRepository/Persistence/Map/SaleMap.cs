using KadoshDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KadoshRepository.Persistence.Map
{
    internal class SaleMap : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FormOfPayment).IsRequired();
            builder.Property(x => x.DiscountInPercentage).IsRequired();
            builder.Property(x => x.DownPayment).IsRequired();
            builder.Property(x => x.SaleDate).IsRequired();
            builder.Property(x => x.Situation).IsRequired();
            builder.Property(x => x.SettlementDate);
            builder.HasOne(x => x.Customer).WithMany(x => x.Sales).HasForeignKey(x => x.CustomerId).IsRequired();
            builder.HasMany(x => x.SaleItems).WithOne(x => x.Sale).HasForeignKey(x => x.SaleId).IsRequired();
            builder.HasMany(x => x.Postings).WithOne(x => x.Sale).HasForeignKey(x => x.SaleId);
            builder.Ignore(x => x.Notifications);

        }
    }
}
