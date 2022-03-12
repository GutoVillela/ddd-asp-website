using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshShared.Constants.ValidationErrors;
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
        /// Used from Entity Framework only.
        /// </summary>
        private SaleInInstallments(
            int customerId,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            ESaleSituation situation,
            int sellerId,
            int storeId,
            decimal interestOnTheTotalSaleInPercentage
            ) : base(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, situation, sellerId, storeId)
        {
            InterestOnTheTotalSaleInPercentage = interestOnTheTotalSaleInPercentage;
            ValidateSaleInInstallments();
        }

        public SaleInInstallments(
            Customer customer,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            ESaleSituation situation,
            User seller,
            Store store,
            IReadOnlyCollection<SaleItem> saleItems,
            IReadOnlyCollection<Installment> installments,
            decimal interestOnTheTotalSaleInPercentage
            ) : base(customer, formOfPayment, discountInPercentage, downPayment, saleDate, situation, seller, store, saleItems)
        {
            Installments = installments;
            InterestOnTheTotalSaleInPercentage = interestOnTheTotalSaleInPercentage;
            ValidateSaleInInstallments();
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

        private void ValidateSaleInInstallments()
        {
            ValidateSale();
            AddNotifications(new Contract<Notification>()
                    .Requires()
                    .IsGreaterOrEqualsThan(InterestOnTheTotalSaleInPercentage, 0, nameof(InterestOnTheTotalSaleInPercentage), SaleValidationsErrors.LESS_THAN_TWO_INSTALLMENTS_ERROR)
                );
            if (Installments is null)
                AddNotification(nameof(Installments), "Não é possível criar uma venda parcelada sem nenhuma parcela!");
            else
                AddNotifications(new Contract<Notification>()
                    .Requires()
                    .IsGreaterOrEqualsThan(Installments.Count, 2, nameof(Installments), "É necessária pelo menos duas parcelas em uma venda parcelada!")
                );
        }
    }
}
