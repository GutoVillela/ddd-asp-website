using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshDomain.ValueObjects;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands
{
    public class UpdateCategoryCommand : Notifiable<Notification>, ICommand
    {
        public int? Id { get; set; }
        public string? Name { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(Id, nameof(Id), CategoryValidationsErrors.INVALID_CATEGORY_ID)
                .IsNotNullOrEmpty(Name, nameof(Name), CategoryValidationsErrors.INVALID_CATEGORY_NAME)
            );
        }
    }
}
