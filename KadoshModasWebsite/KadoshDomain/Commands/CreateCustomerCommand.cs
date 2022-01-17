using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshDomain.ValueObjects;
using KadoshShared.Commands;

namespace KadoshDomain.Commands
{
    public class CreateCustomerCommand : Notifiable<Notification>, ICommand
    {
        public string? Name { get; set; }
        public string? EmailAddress { get; set; }
        public string? DocumentNumber { get; set; }
        public EDocumentType DocumentType { get; set; }
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
                .IsNotNullOrEmpty(Name, nameof(Name), "Nome do cliente inválido!")
            );
        }
    }
}
