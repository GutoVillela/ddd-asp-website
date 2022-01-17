using KadoshDomain.Enums;

namespace KadoshDomain.Entities
{
    public class SaleInCash : Sale
    {
        #region Constructors
        /// <summary>
        /// Private constructor used from Entity Framework.
        /// </summary>
        private SaleInCash(int customerId, decimal discountInPercentage, decimal downPayment, DateTime saleDate, ESaleSituation situation) : base(customerId, discountInPercentage, downPayment, saleDate, situation)
        {

        }

        public SaleInCash(
            int customerId,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            IReadOnlyCollection<SaleItem> saleItems,
            ESaleSituation situation
            ) : base(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation)
        {
            
        }

        protected SaleInCash(
            int customerId,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            IReadOnlyCollection<SaleItem> saleItems,
            ESaleSituation situation,
            DateTime settlementDate
            ) : base(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation, settlementDate)
        {
            
        }

        protected SaleInCash(
            int customerId,
            EFormOfPayment formOfPayment,
            decimal discountInPercentage,
            decimal downPayment,
            DateTime saleDate,
            IReadOnlyCollection<SaleItem> saleItems,
            ESaleSituation situation,
            DateTime settlementDate,
            IReadOnlyCollection<CustomerPosting> postings
            ) : base(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation, settlementDate, postings)
        {
        }

        protected SaleInCash(
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
            ) : base(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation, settlementDate, postings, customer)
        {
        }
        #endregion Constructors
    }
}
