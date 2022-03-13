using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.StoreQueries.GetStoreById
{
    public class GetStoreByIdQuery : Notifiable<Notification>, IQueryRequest
    {
        public int? StoreId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(StoreId, nameof(StoreId), StoreValidationErrors.INVALID_STORE_ID)
            );
        }
    }
}
