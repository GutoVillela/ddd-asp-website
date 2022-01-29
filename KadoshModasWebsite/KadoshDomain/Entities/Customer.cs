using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshDomain.ValueObjects;
using KadoshShared.Entities;
using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.Entities
{
    public class Customer : Entity
    {
        #region Constructors
        public Customer(string name)
        {
            Name = name;

            ValidateCustomer();
        }

        public Customer(string name, Email? email) : this(name)
        {
            Email = email;
        }

        public Customer(string name, Email? email, Document? document) : this(name, email)
        {
            Document = document;
        }

        public Customer(string name, Email? email, Document? document, EGender? gender) : this(name, email, document)
        {
            Gender = gender;
        }

        public Customer(string name, Email? email, Document? document, EGender? gender, Address? address) : this(name, email, document, gender)
        {
            Address = address;
        }

        public Customer(string name, Email? email, Document? document, EGender? gender, Address? address, IReadOnlyCollection<Phone>? phones) : this(name, email, document, gender, address)
        {
            Phones = phones;
        }

        public Customer(
            string name,
            Email? email, 
            Document? document,
            EGender? gender,
            Address? address, 
            IReadOnlyCollection<Phone>? phones,
            IReadOnlyCollection<Sale> sales,
            IReadOnlyCollection<CustomerPosting> customerPostings
            ) : this(name, email, document, gender, address, phones)
        {
            Sales = sales;
            CustomerPostings = customerPostings;
        }

        public Customer(
            int id,
            string name,
            Email? email,
            Document? document,
            EGender? gender,
            Address? address,
            IReadOnlyCollection<Phone>? phones
            ) : this(name, email, document, gender, address, phones)
        {
            Id = id;
        }
        #endregion Constructors

        [Required]
        [MaxLength(255)]
        public string Name { get; private set; }

        public Email? Email { get; private set; }
        public Document? Document { get; private set; }
        public EGender? Gender { get; private set; }
        public Address? Address { get; private set; }
        public IReadOnlyCollection<Phone>? Phones { get; private set; }

        public IReadOnlyCollection<Sale> Sales { get; private set; } =  new List<Sale>();

        public IReadOnlyCollection<CustomerPosting> CustomerPostings { get; private set; } = new List<CustomerPosting>();

        public void UpdateCustomerInfo(
            string name,
            Email? email,
            Document? document,
            EGender? gender,
            Address? address,
            IReadOnlyCollection<Phone>? phones)
        {
            Name = name;
            Email = email;
            Document = document;
            Gender = gender;
            Address = address;
            Phones = phones;
            ValidateCustomer();
        }

        private void ValidateCustomer()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Name, nameof(Name), "Nome do cliente inválido")
            );
        }
    }
}