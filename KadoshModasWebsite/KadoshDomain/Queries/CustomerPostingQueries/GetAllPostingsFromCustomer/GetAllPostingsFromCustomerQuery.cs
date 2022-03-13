using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.CustomerPostingQueries.GetAllPostingsFromCustomer
{
    public class GetAllPostingsFromCustomerQuery : Notifiable<Notification>, IQueryRequest
    {
        public int? CustomerId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(CustomerId, nameof(CustomerId), CustomerPostingValidationsErrors.NULL_CUSTOMER_ID)
            );
        }
    }
}
