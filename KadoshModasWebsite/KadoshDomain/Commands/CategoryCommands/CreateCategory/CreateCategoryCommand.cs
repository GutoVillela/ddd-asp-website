using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands.CategoryCommands.CreateCategory
{
    public class CreateCategoryCommand : Notifiable<Notification>, ICommand
    {
        public string? Name { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Name, nameof(Name), CategoryValidationsErrors.INVALID_CATEGORY_NAME)
            );
        }
    }
}
