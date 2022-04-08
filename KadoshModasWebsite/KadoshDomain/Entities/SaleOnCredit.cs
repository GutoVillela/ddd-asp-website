using KadoshDomain.Enums;

namespace KadoshDomain.Entities
{
    public class SaleOnCredit : Sale
    {
        #region Constructors
        public SaleOnCredit(int customerId, EFormOfPayment formOfPayment, decimal discountInPercentage, decimal downPayment, DateTime saleDate, ESaleSituation situation, int sellerId, int storeId) : base(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, situation, sellerId, storeId, settlementDate: null)
        {

        }
        #endregion Constructors

    }
}
