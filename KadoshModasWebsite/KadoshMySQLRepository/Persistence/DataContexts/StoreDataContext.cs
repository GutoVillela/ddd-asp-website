using KadoshDomain.Entities;
using KadoshRepository.Persistence.Map;
using Microsoft.EntityFrameworkCore;

namespace KadoshRepository.Persistence.DataContexts
{
    public class StoreDataContext : DbContext
    {
        public StoreDataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerPosting> CustomersPostings { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleInCash> SalesInCash { get; set; }
        public DbSet<SaleInInstallments> SalesInInstallments { get; set; }
        public DbSet<SaleOnCredit> SalesOnCredit { get; set; }
        public DbSet<SaleItem> SalesItems { get; set; }
        public DbSet<Installment> Installments { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BrandMap());
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new CustomerMap());
            modelBuilder.ApplyConfiguration(new CustomerPostingMap());
            modelBuilder.ApplyConfiguration(new InstallmentMap());
            modelBuilder.ApplyConfiguration(new ProductMap());
            modelBuilder.ApplyConfiguration(new SaleInCashMap());
            modelBuilder.ApplyConfiguration(new SaleInInstallmentsMap());
            modelBuilder.ApplyConfiguration(new SaleItemMap());
            modelBuilder.ApplyConfiguration(new SaleMap());
            modelBuilder.ApplyConfiguration(new SaleOnCreditMap());
            modelBuilder.ApplyConfiguration(new StockMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new StoreMap());
        }

    }
}
