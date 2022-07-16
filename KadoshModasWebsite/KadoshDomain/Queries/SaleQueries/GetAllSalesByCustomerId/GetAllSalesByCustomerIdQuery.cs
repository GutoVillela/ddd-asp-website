using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.SaleQueries.GetAllSalesByCustomerId
{
    public class GetAllSalesByCustomerIdQuery : Notifiable<Notification>, IQueryRequest
    {
        public int? CustomerId { get; set; }

        /// <summary>
        /// If the value is zero the query will fetch all sales.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// If the value is zero the query will fetch all sales.
        /// </summary>
        public int PageSize { get; set; }

        public bool IncludeProductsInfo { get; set; } = false;

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(CustomerId, nameof(CustomerId), SaleValidationsErrors.INVALID_SALE_CUSTOMER)
                .IsGreaterOrEqualsThan(CurrentPage, 0, nameof(CurrentPage), SaleValidationsErrors.QUERY_CURRENT_PAGE_LOWER_THAN_ZERO)
                .IsGreaterOrEqualsThan(PageSize, 0, nameof(PageSize), SaleValidationsErrors.QUERY_PAGE_SIZE_LOWER_THAN_ZERO)
            );
        }
    }
}
