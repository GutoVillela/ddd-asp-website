using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshDomain.ValueObjects;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands
{
    public class CreateProductCommand : Notifiable<Notification>, ICommand
    {
        public string? Name { get; set; }

        public string? BarCode { get; set; }

        public decimal? Price { get; set; }

        public int? CategoryId { get; set; }

        public int? BrandId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Name, nameof(Name), ProductValidationsErrors.INVALID_PRODUCT_NAME)
                .IsNotNull(Price, nameof(Price), ProductValidationsErrors.INVALID_PRODUCT_PRICE)
                .IsGreaterThan(Price ?? 0, 0, nameof(Price), ProductValidationsErrors.PRODUCT_PRICE_IS_ZERO)
                .IsNotNull(CategoryId, nameof(CategoryId), ProductValidationsErrors.INVALID_PRODUCT_CATEGORY)
                .IsNotNull(BrandId, nameof(BrandId), ProductValidationsErrors.INVALID_PRODUCT_BRAND)
            );
        }
    }
}
