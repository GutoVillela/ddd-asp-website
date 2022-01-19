using KadoshDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KadoshRepository.Persistence.Map
{
    internal class StoreMap : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
            builder.OwnsOne(x => x.Address,
                address =>
                {
                    address.Property(a => a.Street);
                    address.Property(a => a.Number);
                    address.Property(a => a.Neighborhood);
                    address.Property(a => a.City);
                    address.Property(a => a.State);
                    address.Property(a => a.ZipCode);
                    address.Property(a => a.Complement);
                    address.Ignore(a => a.Notifications);
                });
            builder.HasMany(x => x.Users).WithOne(x => x.Store).HasForeignKey(x => x.StoreId);
            builder.HasMany(x => x.Stocks).WithOne(x => x.Store).HasForeignKey(x => x.StoreId);
            builder.Ignore(x => x.Notifications);
        }
    }
}
