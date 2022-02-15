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
        public SaleOnCredit(int customerId, EFormOfPayment formOfPayment, decimal discountInPercentage, decimal downPayment, DateTime saleDate, ESaleSituation situation, int sellerId) : base(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, situation, sellerId, settlementDate: null)
        {

        }

        //public SaleOnCredit(
        //    int customerId,
        //    EFormOfPayment formOfPayment,
        //    decimal discountInPercentage,
        //    decimal downPayment,
        //    DateTime saleDate,
        //    IReadOnlyCollection<SaleItem> saleItems,
        //    ESaleSituation situation,
        //    int sellerId
        //    ) : base(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation, sellerId)
        //{

        //}

        //protected SaleOnCredit(
        //    int customerId,
        //    EFormOfPayment formOfPayment,
        //    decimal discountInPercentage,
        //    decimal downPayment,
        //    DateTime saleDate,
        //    IReadOnlyCollection<SaleItem> saleItems,
        //    ESaleSituation situation,
        //    string sellerId,
        //    DateTime settlementDate
        //    ) : base(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation, sellerId, settlementDate)
        //{

        //}

        //protected SaleOnCredit(
        //    int customerId,
        //    EFormOfPayment formOfPayment,
        //    decimal discountInPercentage,
        //    decimal downPayment,
        //    DateTime saleDate,
        //    IReadOnlyCollection<SaleItem> saleItems,
        //    ESaleSituation situation,
        //    string sellerId,
        //    DateTime settlementDate,
        //    IReadOnlyCollection<CustomerPosting> postings
        //    ) : base(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation, sellerId, settlementDate, postings)
        //{
        //}

        //protected SaleOnCredit(
        //    int customerId,
        //    EFormOfPayment formOfPayment,
        //    decimal discountInPercentage,
        //    decimal downPayment,
        //    DateTime saleDate,
        //    IReadOnlyCollection<SaleItem> saleItems,
        //    ESaleSituation situation,
        //    string sellerId,
        //    DateTime settlementDate,
        //    IReadOnlyCollection<CustomerPosting> postings,
        //    Customer? customer
        //    ) : base(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, saleItems, situation, sellerId, settlementDate, postings, customer)
        //{
        //}
        #endregion Constructors

    }
}
