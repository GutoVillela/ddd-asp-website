using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.ProductQueries.GetProductByBarCode
{
    public class GetProductByBarCodeQuery : Notifiable<Notification>, IQueryRequest
    {
        public string BarCode { get; set; } = string.Empty;

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(BarCode, nameof(BarCode), ProductValidationsErrors.PRODUCT_BARCODE_NOT_GIVEN_IN_QUERY)
            );
        }
    }
}
