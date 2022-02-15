using KadoshDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KadoshRepository.Persistence.Map
{
    internal class SaleItemMap : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.HasKey(x => new { x.SaleId, x.ProductId });
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.DiscountInPercentage).IsRequired();
            builder.Property(x => x.Situation).IsRequired();
            builder.HasOne(x => x.Sale).WithMany(x => x.SaleItems).HasForeignKey(x => x.SaleId);
            builder.HasOne(x => x.Product).WithMany(x => x.SaleItems).HasForeignKey(x => x.ProductId);
            builder.Ignore(x => x.Id);
            builder.Ignore(x => x.Notifications);
        }
    }
}
