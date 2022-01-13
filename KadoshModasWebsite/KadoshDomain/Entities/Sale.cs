using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshShared.Entities;

namespace KadoshDomain.Entities
{
    public abstract class Sale : Entity
    {
        #region Attributes
        private readonly IList<CustomerPosting> _postings;
        #endregion Attributes

        #region Constructor
        protected Sale(
            Customer customer,
            EFormOfPayment formOfPayment, 
            decimal discountInPercentage, 
            decimal downPayment, 
            DateTime saleDate, 
            IReadOnlyCollection<SaleItem> saleItems, 
            ESaleSituation situation
            )
        {
            Customer = customer;
            FormOfPayment = formOfPayment;
            DiscountInPercentage = discountInPercentage;
            DownPayment = downPayment;
            SaleDate = saleDate;
            SaleItems = saleItems;
            Situation = situation;
            _postings = new List<CustomerPosting>();

            ValidateSaleWithNoSettlementDate();
        }

        protected Sale(
            Customer customer,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            IReadOnlyCollection<SaleItem> saleItems,
            ESaleSituation situation,
            DateTime settlementDate
            ) : this(customer, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation)
        {
            SettlementDate = settlementDate;

            ValidateSettlementDate();
        }
        #endregion Constructor

        #region Properties
        public Customer Customer { get; private set; }
        public EFormOfPayment FormOfPayment { get; private set; }
        public decimal DiscountInPercentage { get; private set; }
        public decimal DownPayment { get; private set; }
        public DateTime SaleDate { get; private set; }
        public DateTime? SettlementDate { get; private set; }
        public IReadOnlyCollection<SaleItem> SaleItems { get; private set; }
        public ESaleSituation Situation { get; private set; }
        public decimal Total { get { return CalculateTotal(); } }
        public IReadOnlyCollection<CustomerPosting> Postings { get { return _postings.ToList(); } }

        #endregion Properties

        #region Methods
        private void ValidateSaleWithNoSettlementDate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsBetween(DiscountInPercentage, 0, 100, nameof(DiscountInPercentage), "O desconto da venda precisa estar entre 0 e 100%!")
                .IsGreaterOrEqualsThan(DownPayment, 0, nameof(DownPayment), "O valor da entrada não pode ser menor do que 0!")
                .IsLowerThan(DownPayment, CalculateTotal(), nameof(DownPayment), "O valor da entrada precisa ser menor que o valor total da venda!")
                .IsTrue(SaleItems.Any(), nameof(SaleItems), "É obrigatório pelo menos um item da venda!")
            );

            ValidateSaleItems();
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
            if(SettlementDate.HasValue)
                AddNotifications(new Contract<Notification>()
                    .Requires()
                    .IsGreaterOrEqualsThan(SettlementDate.Value, SaleDate, nameof(SettlementDate), "A data de conclusão da venda não pode ser anterior à data da venda!")
                );
        }

        private decimal CalculateTotal()
        {
            decimal total = decimal.Zero;

            foreach (var item in SaleItems)
            {
                if (item.Situation is not ESaleItemSituation.Canceled and not ESaleItemSituation.Exchanged)
                    total += (item.Price - item.Price * (item.DiscountInPercentage / 100)) * item.Amount;
            }

            total -= total * (DiscountInPercentage / 100);

            return total;
        }

        public void AddPosting(CustomerPosting posting)
        {
            _postings.Add(posting);
        }
        #endregion Methods
    }
}
