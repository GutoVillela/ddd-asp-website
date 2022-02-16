using KadoshDomain.Enums;

namespace KadoshDomain.Entities
{
    public class SaleInCash : Sale
    {
        #region Constructors
        public SaleInCash(int customerId, EFormOfPayment formOfPayment, decimal discountInPercentage, decimal downPayment, DateTime saleDate, ESaleSituation situation, int sellerId, int storeId,  DateTime? settlementDate) : base(customerId, formOfPayment, discountInPercentage, downPayment, saleDate, situation, sellerId, storeId, settlementDate)
        {

        }

        //protected SaleInCash(
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

        //protected SaleInCash(
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

        //protected SaleInCash(
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
