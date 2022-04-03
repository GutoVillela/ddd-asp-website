using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.SaleQueries.GetSalesOfTheWeek
{
    public class GetSalesOfTheWeekQuery : Notifiable<Notification>, IQueryRequest
    {
        /// <summary>
        /// Local Timezone where the week should be calculated.
        /// </summary>
        public TimeZoneInfo? LocalTimeZone { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(LocalTimeZone, nameof(LocalTimeZone), SaleValidationsErrors.NULL_QUERY_LOCAL_TIMEZONE)
            );
        }
    }
}
