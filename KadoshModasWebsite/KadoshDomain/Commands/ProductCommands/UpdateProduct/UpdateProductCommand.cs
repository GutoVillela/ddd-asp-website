using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands.ProductCommands.UpdateProduct
{
    public class UpdateProductCommand : Notifiable<Notification>, ICommand
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? BarCode { get; set; }

        public decimal? Price { get; set; }

        public int? CategoryId { get; set; }

        public int? BrandId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(Id, nameof(Id), ProductValidationsErrors.INVALID_PRODUCT_ID)
                .IsNotNullOrEmpty(Name, nameof(Name), ProductValidationsErrors.INVALID_PRODUCT_NAME)
                .IsNotNull(Price, nameof(Price), ProductValidationsErrors.INVALID_PRODUCT_PRICE)
                .IsGreaterThan(Price ?? 0, 0, nameof(Price), ProductValidationsErrors.PRODUCT_PRICE_IS_ZERO)
                .IsNotNull(CategoryId, nameof(CategoryId), ProductValidationsErrors.INVALID_PRODUCT_CATEGORY)
                .IsNotNull(BrandId, nameof(BrandId), ProductValidationsErrors.INVALID_PRODUCT_BRAND)
            );
        }
    }
}
