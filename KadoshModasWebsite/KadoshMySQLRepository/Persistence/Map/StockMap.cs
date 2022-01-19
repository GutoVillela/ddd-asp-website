using KadoshDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KadoshRepository.Persistence.Map
{
    internal class StockMap : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AmountInStock).IsRequired();
            builder.Property(x => x.MinimumAmountBeforeLowStock).IsRequired();
            builder.HasOne(x => x.Product);
            builder.HasOne(x => x.Store).WithMany(x => x.Stocks).HasForeignKey(x => x.StoreId).IsRequired();
            builder.Ignore(x => x.Notifications);
        }
    }
}
