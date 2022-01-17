using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KadoshDomain.Entities
{
    public class SaleInInstallments : Sale
    {

        #region Constructors
        /// <summary>
        /// Private constructor used from Entity Framework.
        /// </summary>
        private SaleInInstallments(int customerId, decimal discountInPercentage, decimal downPayment, DateTime saleDate, ESaleSituation situation) : base(customerId, discountInPercentage, downPayment, saleDate, situation)
        {

        }

        public SaleInInstallments(
            int customerId,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            IReadOnlyCollection<SaleItem> saleItems,
            ESaleSituation situation,
            IReadOnlyCollection<Installment> installments,
            decimal interestOnTheTotalSaleInPercentage
            ) : base(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation)
        {
            InitiateClass(installments, interestOnTheTotalSaleInPercentage);
        }

        public SaleInInstallments(
            int customerId,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            IReadOnlyCollection<SaleItem> saleItems,
            ESaleSituation situation,
            DateTime settlementDate,
            IReadOnlyCollection<Installment> installments,
            decimal interestOnTheTotalSaleInPercentage
            ) : base(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation, settlementDate)
        {
            InitiateClass(installments, interestOnTheTotalSaleInPercentage);
        }

        public SaleInInstallments(
            int customerId,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            IReadOnlyCollection<SaleItem> saleItems,
            ESaleSituation situation,
            DateTime settlementDate,
            IReadOnlyCollection<CustomerPosting> postings,
            IReadOnlyCollection<Installment> installments,
            decimal interestOnTheTotalSaleInPercentage
            ) : base(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation, settlementDate, postings)
        {
            InitiateClass(installments, interestOnTheTotalSaleInPercentage);
        }

        public SaleInInstallments(
            int customerId,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            IReadOnlyCollection<SaleItem> saleItems,
            ESaleSituation situation,
            DateTime settlementDate,
            IReadOnlyCollection<CustomerPosting> postings,
            Customer? customer,
            IReadOnlyCollection<Installment> installments,
            decimal interestOnTheTotalSaleInPercentage
            ) : base(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation, settlementDate, postings, customer)
        {
            InitiateClass(installments, interestOnTheTotalSaleInPercentage);
        }
        #endregion Constructors

        public int NumberOfInstallments 
        { 
            get 
            {
                if (Installments is null)
                    return 0;

                return Installments.Count;
                
            } 
        }

        [Required]
        public decimal InterestOnTheTotalSaleInPercentage { get; private set; }

        [Required]
        [MinLength(2)]
        public IReadOnlyCollection<Installment> Installments { get; private set; }

        private void InitiateClass(IReadOnlyCollection<Installment> installments, decimal interestOnTheTotalSaleInPercentage)
        {
            Installments = installments;
            InterestOnTheTotalSaleInPercentage = interestOnTheTotalSaleInPercentage;
            ValidateSaleInInstallments();
        }

        private void ValidateSaleInInstallments()
        {
            if(Installments is null)
                AddNotification(nameof(Installments), "Não é possível criar uma venda parcelada sem nenhuma parcela!");
            else
                AddNotifications(new Contract<Notification>()
                    .Requires()
                    .IsGreaterOrEqualsThan(Installments.Count, 2, nameof(Installments), "É necessária pelo menos duas parcelas em uma venda parcelada!")
                );
        }
    }
}
