using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshDomain.ValueObjects;
using KadoshShared.Entities;

namespace KadoshDomain.Entities
{
    public class Customer : Entity
    {
        #region Constructor
        public Customer(string name)
        {
            Name = name;

            ValidateCustomer();
        }

        public Customer(string name, Email email) : this(name)
        {
            Email = email;
        }

        public Customer(string name, Email email, Document document) : this(name, email)
        {
            Document = document;
        }

        public Customer(string name, Email email, Document document, EGender gender) : this(name, email, document)
        {
            Gender = gender;
        }

        public Customer(string name, Email email, Document document, EGender gender, Address address) : this(name, email, document, gender)
        {
            Address = address;
        }

        public Customer(string name, Email email, Document document, EGender gender, Address address, IReadOnlyCollection<Phone> phones) : this(name, email, document, gender, address)
        {
            Phones = phones;
        }
        #endregion Constructor

        #region Properties
        public string Name { get; private set; }
        public Email? Email { get; private set; }
        public Document? Document { get; private set; }
        public EGender? Gender { get; private set; }
        public Address? Address { get; private set; }
        public IReadOnlyCollection<Phone>? Phones { get; private set; }
        #endregion Properties

        #region Methods
        private void ValidateCustomer()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Name, nameof(Name), "Nome do cliente inválido!")
            );
        }
        #endregion Methods
    }
}