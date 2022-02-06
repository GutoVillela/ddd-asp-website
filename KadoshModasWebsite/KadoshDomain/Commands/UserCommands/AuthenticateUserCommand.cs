using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands
{
    public class AuthenticateUserCommand : Notifiable<Notification>, ICommand
    {

        public string? Username { get; set; }
        public string? Password { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Username, nameof(Username), UserValidationsErrors.INVALID_USER_USERNAME)
                .IsNotNullOrEmpty(Password, nameof(Password), UserValidationsErrors.INVALID_USER_PASSWORD)
            );
        }
    }
}
