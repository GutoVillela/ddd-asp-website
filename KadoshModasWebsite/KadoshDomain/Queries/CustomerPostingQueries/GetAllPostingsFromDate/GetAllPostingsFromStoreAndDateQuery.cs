using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.CustomerPostingQueries.GetAllPostingsFromDate
{
    public class GetAllPostingsFromStoreAndDateQuery : Notifiable<Notification>, IQueryRequest
    {
        public DateOnly? LocalDate { get; set; }

        public int? StoreId { get; set; }

        public bool GetTotal { get; set; }

        public TimeZoneInfo? LocalTimeZone { get; set; }

        /// <summary>
        /// If the value is zero the query will fetch all customer postings.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// If the value is zero the query will fetch all customer postings.
        /// </summary>
        public int PageSize { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(LocalDate, nameof(LocalDate), CustomerPostingValidationsErrors.NULL_QUERY_DATE)
                .IsNotNull(StoreId, nameof(StoreId), CustomerPostingValidationsErrors.NULL_QUERY_STORE_ID)
                .IsNotNull(LocalTimeZone, nameof(LocalTimeZone), CustomerPostingValidationsErrors.NULL_QUERY_TIMEZONE)
                .IsGreaterOrEqualsThan(CurrentPage, 0, nameof(CurrentPage), CustomerPostingValidationsErrors.QUERY_CURRENT_PAGE_LOWER_THAN_ZERO)
                .IsGreaterOrEqualsThan(PageSize, 0, nameof(PageSize), CustomerPostingValidationsErrors.QUERY_PAGE_SIZE_LOWER_THAN_ZERO)
            );
        }
    }
}
