using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.SaleQueries.GetSaleById
{
    public class GetSaleByIdQuery : Notifiable<Notification>, IQueryRequest
    {
        public int? SaleId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(SaleId, nameof(SaleId), SaleValidationsErrors.NULL_SALE_ID)
            );
        }
    }
}
