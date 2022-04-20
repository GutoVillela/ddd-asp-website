using KadoshDomain.Enums;

namespace KadoshDomain.Entities
{
    public class SaleInCash : Sale
    {
        #region Constructors
        /// <summary>
        /// Userd for Entity Framework only.
        /// </summary>
        private SaleInCash(int customerId, EFormOfPayment formOfPayment, decimal discountInPercentage, decimal downPayment, DateTime saleDate, ESaleSituation situation, int sellerId, int storeId, DateTime? settlementDate) : base(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, situation, sellerId, storeId, settlementDate)
        {

        }

        public SaleInCash(int customerId, EFormOfPayment formOfPayment, decimal discountInPercentage, decimal downPayment, DateTime saleDate, IEnumerable<SaleItem> saleItems, ESaleSituation situation, int sellerId, int storeId, DateTime settlementDate) : base(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems as IReadOnlyCollection<SaleItem>, situation,  sellerId, storeId, settlementDate)
        {

        }
        #endregion Constructors
    }
}
