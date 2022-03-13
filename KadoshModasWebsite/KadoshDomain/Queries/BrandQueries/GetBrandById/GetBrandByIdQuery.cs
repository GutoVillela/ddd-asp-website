using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.BrandQueries.GetBrandById
{
    public class GetBrandByIdQuery : Notifiable<Notification>, IQueryRequest
    {
        public int? BrandId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(BrandId, nameof(BrandId), BrandValidationsErrors.INVALID_BRAND_ID)
            );
        }
    }
}
