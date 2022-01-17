using KadoshDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace KadoshRepository.Persistence.Map
{
    internal class InstallmentMap : IEntityTypeConfiguration<Installment>
    {
        public void Configure(EntityTypeBuilder<Installment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Number).IsRequired();
            builder.Property(x => x.Value).IsRequired();
            builder.Property(x => x.CreationDate).IsRequired();
            builder.Property(x => x.Situation).IsRequired();
            builder.Property(x => x.SettlementDate);
            builder.HasOne(x => x.Sale).WithMany(x => x.Installments).HasForeignKey(x => x.SaleId).IsRequired();
            builder.Ignore(x => x.Notifications);
        }
    }
}
