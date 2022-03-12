using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands.CustomerCommands.DeleteCustomer
{
    public class DeleteCustomerCommand : Notifiable<Notification>, ICommand
    {
        public int? Id { get; set; }
        
        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(Id, nameof(Id), CustomerValidationsErrors.INVALID_CUSTOMER_ID)
            );
        }
    }
}
