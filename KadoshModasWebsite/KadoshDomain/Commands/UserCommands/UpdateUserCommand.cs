using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshDomain.ValueObjects;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands
{
    public class UpdateUserCommand : Notifiable<Notification>, ICommand
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? NewUsername { get; set; }

        public string? OriginalUsername { get; set; }

        public string? Password { get; set; }

        public EUserRole? Role { get; set; }

        public int? StoreId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(Id, nameof(Id), UserValidationsErrors.INVALID_USER_ID)
                .IsNotNullOrEmpty(Name, nameof(Name), UserValidationsErrors.INVALID_USER_NAME)
                .IsNotNullOrEmpty(NewUsername, nameof(NewUsername), UserValidationsErrors.INVALID_USER_USERNAME)
                .IsNotNullOrEmpty(OriginalUsername, nameof(OriginalUsername), UserValidationsErrors.INVALID_USER_ORIGINAL_USERNAME)
                .IsNotNullOrEmpty(Password, nameof(Password), UserValidationsErrors.INVALID_USER_PASSWORD)
                .IsNotNull(Role, nameof(Role), UserValidationsErrors.INVALID_USER_ROLE)
                .IsNotNull(StoreId, nameof(StoreId), UserValidationsErrors.INVALID_USER_STORE)
            );
        }
    }
}
