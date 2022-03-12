using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands.StoreCommands.DeleteStore
{
    public class DeleteStoreCommand : Notifiable<Notification>, ICommand
    {
        public int? Id { get; set; }
        
        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(Id, nameof(Id), StoreValidationErrors.INVALID_STORE_ID)
            );
        }
    }
}
