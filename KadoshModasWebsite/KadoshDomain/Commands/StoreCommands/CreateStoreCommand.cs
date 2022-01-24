using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands
{
    public class CreateStoreCommand : Notifiable<Notification>, ICommand
    {
        public string? Name { get; set; }
        public string? AddressStreet { get; set; }
        public string? AddressNumber { get; set; }
        public string? AddressNeighborhood { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressState { get; set; }
        public string? AddressZipCode { get; set; }
        public string? AddressComplement { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Name, nameof(Name), StoreValidationErrors.INVALID_STORE_NAME_ERROR)
                .IsNotNullOrEmpty(AddressStreet, nameof(AddressStreet), StoreValidationErrors.INVALID_STORE_STREET)
                .IsNotNullOrEmpty(AddressNumber, nameof(AddressNumber), StoreValidationErrors.INVALID_STORE_NUMBER)
            );
        }
    }
}
