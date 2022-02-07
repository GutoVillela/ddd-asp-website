using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshDomain.ValueObjects;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands
{
    public class UpdateBrandCommand : Notifiable<Notification>, ICommand
    {
        public int? Id { get; set; }
        public string? Name { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(Id, nameof(Id), BrandValidationsErrors.INVALID_BRAND_ID)
                .IsNotNullOrEmpty(Name, nameof(Name), BrandValidationsErrors.INVALID_BRAND_NAME)
            );
        }
    }
}
