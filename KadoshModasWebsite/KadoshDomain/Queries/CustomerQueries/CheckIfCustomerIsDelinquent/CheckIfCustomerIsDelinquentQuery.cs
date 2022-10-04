using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.CustomerQueries.CheckIfCustomerIsDelinquent
{
    public class CheckIfCustomerIsDelinquentQuery : Notifiable<Notification>, IQueryRequest
    {
        public int? CustomerId { get; set; }

        public int IntervalSinceLastPaymentInDays { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(CustomerId, nameof(CustomerId), CustomerValidationsErrors.INVALID_CUSTOMER_ID)
                .IsNotNull(IntervalSinceLastPaymentInDays, nameof(IntervalSinceLastPaymentInDays), CustomerValidationsErrors.INVALID_CUSTOMER_ID)
                .IsGreaterThan(IntervalSinceLastPaymentInDays, 0, nameof(IntervalSinceLastPaymentInDays), CustomerValidationsErrors.QUERY_INTERVAL_SINCE_LAST_PAYMENT_LOWER_THAN_OR_EQUALS_ZERO)
            );
        }
    }
}
