using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands.BrandCommands.DeleteBrand
{
    public class DeleteBrandCommand : Notifiable<Notification>, ICommand
    {
        public int? Id { get; set; }
        
        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(Id, nameof(Id), BrandValidationsErrors.INVALID_BRAND_ID)
            );
        }
    }
}
