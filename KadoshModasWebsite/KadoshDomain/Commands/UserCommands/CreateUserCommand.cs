using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshDomain.ValueObjects;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands
{
    public class CreateUserCommand : Notifiable<Notification>, ICommand
    {
        public string? Name { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public EUserRole? Role { get; set; }

        public int? StoreId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Name, nameof(Name), UserValidationsErrors.INVALID_USER_NAME)
                .IsNotNullOrEmpty(Username, nameof(Username), UserValidationsErrors.INVALID_USER_USERNAME)
                .IsNotNullOrEmpty(Password, nameof(Password), UserValidationsErrors.INVALID_USER_PASSWORD)
                .IsNotNull(Role, nameof(Role), UserValidationsErrors.INVALID_USER_ROLE)
                .IsNotNull(StoreId, nameof(StoreId), UserValidationsErrors.INVALID_USER_STORE)
            );
        }
    }
}
