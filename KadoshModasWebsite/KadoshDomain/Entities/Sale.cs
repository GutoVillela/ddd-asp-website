﻿using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshShared.Entities;
using System.ComponentModel.DataAnnotations;
using KadoshDomain.ExtensionMethods;
using KadoshShared.Constants.ValidationErrors;

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
            ValidateSale();
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
            ValidateSale();
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


        public Customer? OriginalCustomer { get; protected set; }

        public int? OriginalCustomerId { get; protected set; }

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
            SettlementDate = settlementDate.ToUniversalTime();
            ValidateSettlementDate();
        }

        public void SetSituation(ESaleSituation situation)
        {
            Situation = situation;
        }

        public decimal Total { get { return CalculateTotal(SaleItems, DiscountInPercentage, Situation); } }

        public decimal TotalPaid { get { return CalculateTotalPaid(); } }

        public decimal TotalToPay { get { return Total - TotalPaid; } }
        public void AddPosting(CustomerPosting posting)
        {
            _postings.Add(posting);
        }

        /// <summary>
        /// Fix the sale status based on the sale total and sale items situation.
        /// </summary>
        public void FixSaleSituationBasedOnTotalAndItems()
        {
            if (Total <= 0 || 
                !SaleItems.Any(x => x.Situation == ESaleItemSituation.AcquiredInExchange || x.Situation == ESaleItemSituation.AcquiredOnPurchase)
            )
                Situation = ESaleSituation.Canceled;
        }

        public void SetCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (!customer.IsValid)
                throw new ArgumentException("Cliente inválido");

            Customer = customer;
            CustomerId = customer.Id;
        }

        public void MergeSaleInCustomer(Customer newCustomer)
        {
            if (newCustomer == null)
                throw new ArgumentNullException(nameof(newCustomer));

            if (!newCustomer.IsValid)
                throw new ArgumentException("Cliente para mesclar inválido");

            //OriginalCustomer = Customer?.Clone() as Customer;
            OriginalCustomerId = CustomerId;
            //Customer = newCustomer;
            CustomerId = newCustomer.Id;
        }

        public void SetSeller(User seller)
        {
            if (seller == null)
                throw new ArgumentNullException(nameof(seller));

            if (!seller.IsValid)
                throw new ArgumentException("Vendedor inválido");

            Seller = seller;
            SellerId = seller.Id;
        }

        public void SetStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            if (!store.IsValid)
                throw new ArgumentException("Loja inválido");

            Store = store;
            StoreId = store.Id;
        }

        /// <summary>
        /// Checks if this sale has late payments.
        /// </summary>
        /// <param name="allowedPaymentDelayInDays">Maximum delay since last payment in days.</param>
        /// <returns>True if this sale has late payments and False otherwise.</returns>
        public bool IsLatePaymentSale(int allowedPaymentDelayInDays)
        {
            DateTime lastCreditPostingDate;

            if (Postings.Any(x => x.Type.IsCreditType()))
                lastCreditPostingDate = Postings.Where(x => x.Type.IsCreditType()).Max(x => x.PostingDate);
            else
                lastCreditPostingDate = SaleDate;

            TimeSpan differenceFromToday = DateTime.UtcNow.Subtract(lastCreditPostingDate);

            return differenceFromToday.TotalDays >= allowedPaymentDelayInDays;
        }

        public static decimal CalculateTotal(IEnumerable<SaleItem> saleItems, decimal discountInPercentage, ESaleSituation saleSituation)
        {
            if (saleItems is null)
                return decimal.Zero;

            decimal total = decimal.Zero;

            foreach (var item in saleItems)
            {
                if (saleSituation is not ESaleSituation.Canceled)
                {
                    if (item.Situation is not ESaleItemSituation.Canceled and not ESaleItemSituation.Exchanged)
                        total += (item.Price - item.Price * (item.DiscountInPercentage / 100)) * item.Amount;
                }
                else
                    total += (item.Price - item.Price * (item.DiscountInPercentage / 100)) * item.Amount;
            }

            total -= total * (discountInPercentage / 100);

            return total;
        }

        protected void ValidateSale()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsBetween(DiscountInPercentage, 0, 100, nameof(DiscountInPercentage), "O desconto da venda precisa estar entre 0 e 100%")
                .IsGreaterOrEqualsThan(DownPayment, 0, nameof(DownPayment), "O valor da entrada não pode ser menor do que 0")
                .IsLowerThan(DownPayment, CalculateTotal(SaleItems, DiscountInPercentage, Situation), nameof(DownPayment), SaleValidationsErrors.DOWN_PAYMENT_GREATER_OR_EQUALS_THAN_TOTAL)
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