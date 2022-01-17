using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshShared.Entities;
using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.Entities
{
    public abstract class Sale : Entity
    {
        private IList<CustomerPosting> _postings;

        #region Constructors
        protected Sale(int customerId, decimal discountInPercentage, decimal downPayment, DateTime saleDate, ESaleSituation situation)
        {

        }

        protected Sale(
            int customerId,
            EFormOfPayment formOfPayment, 
            decimal discountInPercentage, 
            decimal downPayment, 
            DateTime saleDate, 
            IReadOnlyCollection<SaleItem> saleItems, 
            ESaleSituation situation
            )
        {
            CustomerId = customerId;
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
            int customerId,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            IReadOnlyCollection<SaleItem> saleItems,
            ESaleSituation situation,
            DateTime settlementDate
            ) : this(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation)
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
            DateTime settlementDate,
            IReadOnlyCollection<CustomerPosting> postings
            ) : this(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation, settlementDate)
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
            DateTime settlementDate,
            IReadOnlyCollection<CustomerPosting> postings,
            Customer? customer
            ) : this(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation, settlementDate, postings)
        {
            Customer = customer;
        }
        #endregion Constructors

        [Required]
        public Customer? Customer { get; private set; }

        public int CustomerId { get; private set; }

        [Required]
        public EFormOfPayment FormOfPayment { get; private set; }

        [Required]
        [Range(0, 100)]
        public decimal DiscountInPercentage { get; private set; }

        [Required]
        public decimal DownPayment { get; private set; }

        [Required]
        public DateTime SaleDate { get; private set; }

        public DateTime? SettlementDate { get; private set; }

        [Required]
        [MinLength(1)]
        public IReadOnlyCollection<SaleItem> SaleItems { get; private set; }

        [Required]
        public ESaleSituation Situation { get; private set; }


        public IReadOnlyCollection<CustomerPosting> Postings 
        { 
            get { return _postings.ToList(); } 
            private set { _postings = value.ToList(); } 
        }

        public decimal Total { get { return CalculateTotal(); } }

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
    }
}