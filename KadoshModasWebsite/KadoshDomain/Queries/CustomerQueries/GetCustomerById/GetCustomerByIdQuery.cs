using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.CustomerQueries.GetCustomerById
{
    public class GetCustomerByIdQuery : Notifiable<Notification>, IQueryRequest
    {
        public int? CustomerId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(CustomerId, nameof(CustomerId), CustomerValidationsErrors.INVALID_CUSTOMER_ID)
            );
        }
    }
}
