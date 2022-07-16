using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.CustomerQueries.GetCustomerByUsername
{
    public class GetCustomerByUsernameQuery : Notifiable<Notification>, IQueryRequest
    {
        public string? Username { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Username, nameof(Username), CustomerValidationsErrors.INVALID_CUSTOMER_USERNAME)
            );
        }
    }
}
