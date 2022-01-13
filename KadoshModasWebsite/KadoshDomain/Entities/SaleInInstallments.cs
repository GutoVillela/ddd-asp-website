using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KadoshDomain.Entities
{
    public class SaleInInstallments : Sale
    {
        #region Constructor
        public SaleInInstallments(
            Customer customer,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment, 
            DateTime saleDate,
            IReadOnlyCollection<SaleItem> saleItems,
            ESaleSituation situation,
            IReadOnlyCollection<Installment> installments,
            decimal interestOnTheTotalSaleInPercentage) : base(customer, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation)
        {
            Installments = installments;
            InterestOnTheTotalSaleInPercentage = interestOnTheTotalSaleInPercentage;
            ValidateSaleInInstallments();
        }

        public SaleInInstallments(
            Customer customer,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            IReadOnlyCollection<SaleItem> saleItems,
            ESaleSituation situation,
            DateTime settlementDate,
            IReadOnlyCollection<Installment> installments,
            decimal interestOnTheTotalSaleInPercentage) : base(customer, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation, settlementDate)
        {
            Installments = installments;
            InterestOnTheTotalSaleInPercentage = interestOnTheTotalSaleInPercentage;
            ValidateSaleInInstallments();
        }
        #endregion Constructor

        #region Properties
        public int NumberOfInstallments 
        { 
            get 
            {
                if (Installments is null)
                    return 0;

                return Installments.Count;
                
            } 
        }
        public decimal InterestOnTheTotalSaleInPercentage { get; private set; }
        public IReadOnlyCollection<Installment> Installments { get; private set; }
        #endregion Properties

        #region Methods
        private void ValidateSaleInInstallments()
        {
            if(Installments is null)
                AddNotification(nameof(Installments), "Não é possível criar uma venda parcelada sem nenhuma parcela!");
            else
                AddNotifications(new Contract<Notification>()
                    .Requires()
                    .IsTrue(Installments.Any(), nameof(Installments), "É necessária pelo menos uma parcela em uma venda parcelada!")
                );
        }
        #endregion Methods
    }
}
