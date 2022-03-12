using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands.BrandCommands.CreateBrand
{
    public class CreateBrandCommand : Notifiable<Notification>, ICommand
    {
        public string? Name { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Name, nameof(Name), BrandValidationsErrors.INVALID_BRAND_NAME)
            );
        }
    }
}
