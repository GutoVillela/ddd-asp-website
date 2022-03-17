using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.StoreQueries.GetAllStores
{
    public class GetAllStoresQuery : Notifiable<Notification>, IQueryRequest
    {
        /// <summary>
        /// If the value is zero the query will fetch all stores.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// If the value is zero the query will fetch all stores.
        /// </summary>
        public int PageSize { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsGreaterOrEqualsThan(CurrentPage, 0, nameof(CurrentPage), StoreValidationErrors.QUERY_CURRENT_PAGE_LOWER_THAN_ZERO)
                .IsGreaterOrEqualsThan(PageSize, 0, nameof(PageSize), StoreValidationErrors.QUERY_PAGE_SIZE_LOWER_THAN_ZERO)
            );
        }
    }
}
