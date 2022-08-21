using KadoshDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KadoshRepository.Persistence.Map
{
    internal class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Gender);
            builder.Property(x => x.Username).HasMaxLength(20);
            builder.HasIndex(x => x.Username).IsUnique();
            builder.Property(x => x.PasswordHash);
            builder.Property(x => x.PasswordSalt);
            builder.Property(x => x.PasswordSaltIterations);

            builder.OwnsOne(x => x.Email, 
                email =>
                {
                    email.Property(e => e.EmailAddress);
                    email.Ignore(e => e.Notifications);
                });

            builder.OwnsOne(x => x.Document,
                doc =>
                {
                    doc.Property(d => d.Number);
                    doc.Property(d => d.Type);
                    doc.Ignore(d => d.Notifications);
                });

            builder.OwnsOne(x => x.Address,
                address =>
                {
                    address.Property(a => a.Street).IsRequired(false);
                    address.Property(a => a.Number).IsRequired(false);
                    address.Property(a => a.Neighborhood).IsRequired(false);
                    address.Property(a => a.City).IsRequired(false);
                    address.Property(a => a.State).IsRequired(false);
                    address.Property(a => a.ZipCode).IsRequired(false);
                    address.Property(a => a.Complement).IsRequired(false);
                    address.Ignore(a => a.Notifications);
                });

            builder.OwnsMany(x => x.Phones, 
                phone =>
                {
                    phone.Property(p => p.Number);
                    phone.Property(p => p.Type);
                    phone.Property(p => p.AreaCode);
                    phone.Property(p => p.TalkTo);
                    phone.Ignore(p => p.Notifications);
                });
            builder.HasMany(x => x.Sales).WithOne(x => x.Customer).HasForeignKey(x => x.CustomerId);
            builder.HasMany(x => x.BoundedCustomers).WithOne(x => x.IsBoundedTo).HasForeignKey(x => x.BoundedToCustomerId);
            builder.Ignore(x => x.Notifications);
        }
    }
}