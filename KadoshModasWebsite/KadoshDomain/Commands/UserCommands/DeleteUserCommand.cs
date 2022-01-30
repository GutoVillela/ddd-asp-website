using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands
{
    public class DeleteUserCommand : Notifiable<Notification>, ICommand
    {

        public int? Id { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(Id, nameof(Id), UserValidationsErrors.INVALID_USER_ID)
            );
        }
    }
}
