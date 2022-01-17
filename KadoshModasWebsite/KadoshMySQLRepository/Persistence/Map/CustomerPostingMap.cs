using KadoshDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KadoshRepository.Persistence.Map
{
    internal class CustomerPostingMap : IEntityTypeConfiguration<CustomerPosting>
    {
        public void Configure(EntityTypeBuilder<CustomerPosting> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Value).IsRequired();
            builder.Property(x => x.PostingDate).IsRequired();
            builder.HasOne(x => x.Customer).WithMany(x => x.CustomerPostings).HasForeignKey(x => x.CustomerId).IsRequired();
            builder.HasOne(x => x.Sale).WithMany(x => x.Postings).HasForeignKey(x => x.SaleId).IsRequired();
            builder.Ignore(x => x.Notifications);
        }
    }
}
