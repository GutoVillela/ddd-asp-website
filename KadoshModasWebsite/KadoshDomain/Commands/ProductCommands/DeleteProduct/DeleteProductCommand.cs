using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands.ProductCommands.DeleteProduct
{
    public class DeleteProductCommand : Notifiable<Notification>, ICommand
    {
        public int? Id { get; set; }
        
        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(Id, nameof(Id), ProductValidationsErrors.INVALID_PRODUCT_ID)
            );
        }
    }
}
