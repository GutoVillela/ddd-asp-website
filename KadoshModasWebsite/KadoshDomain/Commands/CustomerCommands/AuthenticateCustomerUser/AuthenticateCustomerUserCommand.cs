using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands.CustomerCommands.AuthenticateCustomerUser
{
    public class AuthenticateCustomerUserCommand : Notifiable<Notification>, ICommand
    {

        public string? Username { get; set; }
        public string? Password { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Username, nameof(Username), CustomerValidationsErrors.INVALID_CUSTOMER_USERNAME)
                .IsNotNullOrEmpty(Password, nameof(Password), CustomerValidationsErrors.INVALID_CUSTOMER_PASSWORD)
            );
        }
    }
}
