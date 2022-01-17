using KadoshDomain.Enums;
using KadoshShared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KadoshDomain.Entities
{
    public class SaleOnCredit : Sale
    {
        #region Constructors
        /// <summary>
        /// Private constructor used from Entity Framework.
        /// </summary>
        private SaleOnCredit(int customerId, decimal discountInPercentage, decimal downPayment, DateTime saleDate, ESaleSituation situation) : base(customerId, discountInPercentage, downPayment, saleDate, situation)
        {

        }

        public SaleOnCredit(
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

        protected SaleOnCredit(
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

        protected SaleOnCredit(
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

        protected SaleOnCredit(
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
