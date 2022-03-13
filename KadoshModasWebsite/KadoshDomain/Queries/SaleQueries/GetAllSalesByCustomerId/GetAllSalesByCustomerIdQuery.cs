using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.SaleQueries.GetAllSalesByCustomerId
{
    public class GetAllSalesByCustomerIdQuery : Notifiable<Notification>, IQueryRequest
    {
        public int? CustomerId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(CustomerId, nameof(CustomerId), SaleValidationsErrors.INVALID_SALE_CUSTOMER)
            );
        }
    }
}
