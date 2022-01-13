using KadoshDomain.Enums;

namespace KadoshDomain.Entities
{
    public class SaleInCash : Sale
    {
        #region Constructor
        public SaleInCash(
            Customer customer,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            IReadOnlyCollection<SaleItem> saleItems,
            ESaleSituation situation) : base(customer, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation)
        {
        }

        public SaleInCash(
            Customer customer,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            IReadOnlyCollection<SaleItem> saleItems,
            ESaleSituation situation,
            DateTime settlementDate) : base(customer, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation, settlementDate)
        {
        }
        #endregion Constructor
    }
}
