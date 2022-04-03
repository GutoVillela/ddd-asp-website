using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.CustomerQueries.GetAllDelinquentCustomers
{
    public class GetAllDelinquentCustomersQuery : Notifiable<Notification>, IQueryRequest
    {
        public int IntervalSinceLastPaymentInDays { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(IntervalSinceLastPaymentInDays, nameof(IntervalSinceLastPaymentInDays), CustomerValidationsErrors.INVALID_CUSTOMER_ID)
                .IsGreaterThan(IntervalSinceLastPaymentInDays, 0,nameof(IntervalSinceLastPaymentInDays), CustomerValidationsErrors.QUERY_INTERVAL_SINCE_LAST_PAYMENT_LOWER_THAN_OR_EQUALS_ZERO)
            );
        }
    }
}
