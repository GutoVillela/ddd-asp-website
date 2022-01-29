using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshDomain.ValueObjects;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands
{
    public class UpdateCustomerCommand : Notifiable<Notification>, ICommand
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? EmailAddress { get; set; }
        public string? DocumentNumber { get; set; }
        public EDocumentType? DocumentType { get; set; }
        public EGender? Gender { get; set; }
        public string? AddressStreet { get; set; }
        public string? AddressNumber { get; set; }
        public string? AddressNeighborhood { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressState { get; set; }
        public string? AddressZipCode { get; set; }
        public string? AddressComplement { get; set; }
        public ICollection<Phone>? Phones { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(Id, nameof(Id), CustomerValidationsErrors.INVALID_CUSTOMER_ID)
                .IsNotNullOrEmpty(Name, nameof(Name), CustomerValidationsErrors.INVALID_CUSTOMER_NAME)
            );
        }
    }
}
