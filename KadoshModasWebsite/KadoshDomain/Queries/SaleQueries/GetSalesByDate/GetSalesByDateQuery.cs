using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.SaleQueries.GetSalesByDate
{
    public class GetSalesByDateQuery : Notifiable<Notification>, IQueryRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Local Timezone where the dates should be calculated.
        /// </summary>
        public TimeZoneInfo? LocalTimeZone { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(StartDate, nameof(StartDate), SaleValidationsErrors.NULL_QUERY_START_DATE)
                .IsNotNull(EndDate, nameof(EndDate), SaleValidationsErrors.NULL_QUERY_END_DATE)
                .IsNotNull(LocalTimeZone, nameof(LocalTimeZone), SaleValidationsErrors.NULL_QUERY_LOCAL_TIMEZONE)
            );

            if(StartDate.HasValue && EndDate.HasValue)
            {
                if (StartDate.Value > EndDate.Value)
                    AddNotification(nameof(StartDate), SaleValidationsErrors.QUERY_START_DATE_GREATHER_THAN_END_DATE);
            }
        }
    }
}
