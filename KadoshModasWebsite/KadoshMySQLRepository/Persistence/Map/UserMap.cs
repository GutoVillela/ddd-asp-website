using KadoshDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KadoshRepository.Persistence.Map
{
    internal class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Username);
            builder.Property(x => x.Username).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.Role).IsRequired();
            builder.Ignore(x => x.Notifications);
            builder.Ignore(x => x.Id);
        }
    }
}
