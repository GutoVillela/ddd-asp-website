using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands.CustomerCommands.CreateCustomerUser
{
    public class CreateCustomerUserCommand : Notifiable<Notification>, ICommand
    {
        public int? CustomerId { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(CustomerId, nameof(CustomerId), CustomerValidationsErrors.INVALID_CUSTOMER_ID)
                .IsNotNullOrEmpty(Username, nameof(Username), CustomerValidationsErrors.INVALID_CUSTOMER_USERNAME)
                .IsNotNullOrEmpty(Password, nameof(Password), CustomerValidationsErrors.INVALID_CUSTOMER_PASSWORD)
            );
        }
    }
}
