using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.ProductQueries.GetProductById
{
    public class GetProductByIdQuery : Notifiable<Notification>, IQueryRequest
    {
        public int? ProductId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(ProductId, nameof(ProductId), ProductValidationsErrors.INVALID_PRODUCT_ID)
            );
        }
    }
}
