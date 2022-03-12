using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshShared.Entities;
using System.ComponentModel.DataAnnotations;
using KadoshDomain.ExtensionMethods;

namespace KadoshDomain.Entities
{
    public abstract class Sale : Entity
    {
        protected IList<CustomerPosting> _postings;

        #region Constructors
        protected Sale(int customerId, EFormOfPayment formOfPayment, decimal discountInPercentage, decimal downPayment, DateTime saleDate, ESaleSituation situation, int sellerId, int storeId)
        {
            CustomerId = customerId;
            FormOfPayment = formOfPayment;
            DiscountInPercentage = discountInPercentage;
            DownPayment = downPayment;
            SaleDate = saleDate;
            Situation = situation;
            SellerId = sellerId;
            StoreId = storeId;
            _postings = new List<CustomerPosting>();
        }

        protected Sale(Customer customer, EFormOfPayment formOfPayment, decimal discountInPercentage, decimal downPayment, DateTime saleDate, ESaleSituation situation, User seller, Store store, IReadOnlyCollection<SaleItem> saleItems)
        {
            Customer = customer;
            FormOfPayment = formOfPayment;
            DiscountInPercentage = discountInPercentage;
            DownPayment = downPayment;
            SaleDate = saleDate;
            Situation = situation;
            Seller = seller;
            Store = store;
            SaleItems = saleItems;
            _postings = new List<CustomerPosting>();
        }

        protected Sale(
            int customerId,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            ESaleSituation situation,
            int sellerId,
            int storeId,
            DateTime? settlementDate
            ) : this(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, situation, sellerId, storeId)
        {
            SettlementDate = settlementDate;

            ValidateSettlementDate();
        }

        protected Sale(
            int customerId,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            IReadOnlyCollection<SaleItem> saleItems,
            ESaleSituation situation,
            int sellerId,
            int storeId
            )
        {
            CustomerId = customerId;
            FormOfPayment = formOfPayment;
            DiscountInPercentage = discountInPercentage;
            DownPayment = downPayment;
            SaleDate = saleDate;
            SaleItems = saleItems;
            Situation = situation;
            SellerId = sellerId;
            StoreId = storeId;
            _postings = new List<CustomerPosting>();

            ValidateSale();
        }

        protected Sale(
            int customerId,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            IReadOnlyCollection<SaleItem> saleItems,
            ESaleSituation situation,
            int sellerId,
            int storeId,
            DateTime settlementDate
            ) : this(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation, sellerId, storeId)
        {
            SettlementDate = settlementDate;
        }

        protected Sale(
            int customerId,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            IReadOnlyCollection<SaleItem> saleItems,
            ESaleSituation situation,
            int sellerId,
            int storeId,
            DateTime settlementDate,
            IReadOnlyCollection<CustomerPosting> postings
            ) : this(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation, sellerId, storeId, settlementDate)
        {
            Postings = postings;
        }

        protected Sale(
            int customerId,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            IReadOnlyCollection<SaleItem> saleItems,
            ESaleSituation situation,
            int sellerId,
            int storeId,
            DateTime settlementDate,
            IReadOnlyCollection<CustomerPosting> postings,
            Customer? customer
            ) : this(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation, sellerId, storeId, settlementDate, postings)
        {
            Customer = customer;
        }
        #endregion Constructors

        [Required]
        public Customer? Customer { get; protected set; }

        public int CustomerId { get; protected set; }

        [Required]
        public EFormOfPayment FormOfPayment { get; protected set; }

        [Required]
        [Range(0, 100)]
        public decimal DiscountInPercentage { get; protected set; }

        [Required]
        public decimal DownPayment { get; protected set; }

        [Required]
        public DateTime SaleDate { get; protected set; }

        [Required]
        public int SellerId { get; protected set; }

        public User? Seller { get; protected set; }

        [Required]
        public int StoreId { get; protected set; }

        public Store? Store { get; protected set; }

        public DateTime? SettlementDate { get; protected set; }

        [Required]
        [MinLength(1)]
        public IReadOnlyCollection<SaleItem> SaleItems { get; protected set; }

        [Required]
        public ESaleSituation Situation { get; protected set; }


        public IReadOnlyCollection<CustomerPosting> Postings
        {
            get { return _postings.ToList(); }
            protected set { _postings = value.ToList(); }
        }

        public void SetSaleItems(IEnumerable<SaleItem> saleItems)
        {
            SaleItems = (IReadOnlyCollection<SaleItem>)saleItems;
        }

        public void SetSettlementDate(DateTime settlementDate)
        {
            settlementDate = settlementDate.ToUniversalTime();
            ValidateSettlementDate();
        }

        public void SetSituation(ESaleSituation situation)
        {
            Situation = situation;
        }

        public decimal Total { get { return CalculateTotal(SaleItems, DiscountInPercentage); } }

        public decimal TotalPaid { get { return CalculateTotalPaid(); } }

        public decimal TotalToPay { get { return Total - TotalPaid; } }
        public void AddPosting(CustomerPosting posting)
        {
            _postings.Add(posting);
        }

        protected void ValidateSale()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsBetween(DiscountInPercentage, 0, 100, nameof(DiscountInPercentage), "O desconto da venda precisa estar entre 0 e 100%")
                .IsGreaterOrEqualsThan(DownPayment, 0, nameof(DownPayment), "O valor da entrada não pode ser menor do que 0")
                .IsLowerThan(DownPayment, CalculateTotal(SaleItems, DiscountInPercentage), nameof(DownPayment), "O valor da entrada precisa ser menor que o valor total da venda")
                .IsNotNull(SellerId, nameof(SellerId), "É obrigatório informar o vendedor da venda")
            );

            ValidateSaleItems();
            ValidateSettlementDate();
        }

        private void ValidateSaleItems()
        {
            if (SaleItems is null)
                AddNotification(nameof(SaleItems), "Os itens da venda não podem ser nulos para esta venda!");
            else
                AddNotifications(new Contract<Notification>()
                    .Requires()
                    .IsTrue(SaleItems.Any(), nameof(SaleItems), "É obrigatório pelo menos um item da venda!")
                );
        }

        private void ValidateSettlementDate()
        {
            if (SettlementDate.HasValue)
                AddNotifications(new Contract<Notification>()
                    .Requires()
                    .IsGreaterOrEqualsThan(SettlementDate.Value, SaleDate, nameof(SettlementDate), "A data de conclusão da venda não pode ser anterior à data da venda!")
                );
        }

        public static decimal CalculateTotal(IEnumerable<SaleItem> saleItems, decimal discountInPercentage)
        {
            if(saleItems is null)
                return decimal.Zero;

            decimal total = decimal.Zero;

            foreach (var item in saleItems)
            {
                if (item.Situation is not ESaleItemSituation.Canceled and not ESaleItemSituation.Exchanged)
                    total += (item.Price - item.Price * (item.DiscountInPercentage / 100)) * item.Amount;
            }

            total -= total * (discountInPercentage / 100);

            return total;
        }

        protected decimal CalculateTotalPaid()
        {
            if (!Postings.Any())
                return 0;
            
            decimal totalPaid = 0;

            foreach(var posting in Postings)
            {
                if (posting.IsCreditType())
                    totalPaid += posting.Value;
            }
            return totalPaid;
        }
    }
}