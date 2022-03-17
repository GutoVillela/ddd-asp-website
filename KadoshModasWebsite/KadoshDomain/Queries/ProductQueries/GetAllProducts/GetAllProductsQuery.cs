using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.ProductQueries.GetAllProducts
{
    public class GetAllProductsQuery : Notifiable<Notification>, IQueryRequest
    {
        /// <summary>
        /// If the value is zero the query will fetch all products.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// If the value is zero the query will fetch all products.
        /// </summary>
        public int PageSize { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsGreaterOrEqualsThan(CurrentPage, 0, nameof(CurrentPage), ProductValidationsErrors.QUERY_CURRENT_PAGE_LOWER_THAN_ZERO)
                .IsGreaterOrEqualsThan(PageSize, 0, nameof(PageSize), ProductValidationsErrors.QUERY_PAGE_SIZE_LOWER_THAN_ZERO)
            );
        }
    }
}
