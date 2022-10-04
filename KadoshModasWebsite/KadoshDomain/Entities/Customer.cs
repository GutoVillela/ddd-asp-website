using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshDomain.ValueObjects;
using KadoshShared.Entities;
using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.Entities
{
    public class Customer : Entity, ICloneable
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

        public Customer(
            int id,
            string name,
            Email? email,
            Document? document,
            EGender? gender,
            Address? address,
            string username,
            string passwordHash,
            byte[] passwordSalt,
            int passwordSaltIterations
            ) : this(name, email, document, gender, address)
        {
            Id = id;
            Username = username;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            PasswordSaltIterations = passwordSaltIterations;
        }

        private Customer(
            int id,
            string name,
            Email? email,
            Document? document,
            EGender? gender,
            Address? address,
            string? username,
            string? passwordHash,
            byte[]? passwordSalt,
            int? passwordSaltIterations,
            Customer? isBoundedTo,
            IReadOnlyCollection<Phone>? phones,
            IReadOnlyCollection<Sale> sales,
            IReadOnlyCollection<Customer>? boundedCustomers,
            IReadOnlyCollection<Sale> originalSales
            ) : this(name, email, document, gender, address)
        {
            Id = id;
            Username = username;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            PasswordSaltIterations = passwordSaltIterations;
            IsBoundedTo = isBoundedTo;
            BoundedToCustomerId = isBoundedTo?.Id;
            Phones = phones;
            Sales = sales;
            BoundedCustomers = boundedCustomers;
            OriginalSales = originalSales;

        }
        #endregion Constructors

        [Required]
        [MaxLength(255)]
        public string Name { get; private set; }

        public Email? Email { get; private set; }
        public Document? Document { get; private set; }
        public EGender? Gender { get; private set; }
        public Address? Address { get; private set; }

        [MaxLength(20)]
        public string? Username { get; private set; }

        public string? PasswordHash { get; private set; }

        public byte[]? PasswordSalt { get; private set; }

        public int? PasswordSaltIterations { get; private set; }

        public int? BoundedToCustomerId { get; private set; }

        public Customer? IsBoundedTo { get; private set; }

        public IReadOnlyCollection<Phone>? Phones { get; private set; }

        public IReadOnlyCollection<Sale> Sales { get; private set; } =  new List<Sale>();

        public IReadOnlyCollection<Customer>? BoundedCustomers { get; private set; }

        public IReadOnlyCollection<Sale> OriginalSales { get; private set; } = new List<Sale>();

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

        public void ClearCustomerUsernameAndPasswordData()
        {
            Username = null;
            PasswordHash = null;
            PasswordSalt = null;
            PasswordSaltIterations = null;
        }

        public void SetUsernameAndPassword(string username, string passwordHash, byte[] passwordSalt, int passwordSaltIterations)
        {
            Username = username;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            PasswordSaltIterations = passwordSaltIterations;
        }

        public void MergeToCustomer(Customer mainCustomer)
        {
            IsBoundedTo = mainCustomer;
            foreach(var sale in Sales)
            {
                sale.MergeSaleInCustomer(mainCustomer);
            }
        }

        public bool IsDelinquentCustomer(int allowedPaymentDelayInDays)
        {
            if(!Sales.Any())
                return false;

            return Sales.Any(x => x.IsLatePaymentSale(allowedPaymentDelayInDays));
        }

        private void ValidateCustomer()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Name, nameof(Name), "Nome do cliente inválido")
            );
        }

        public object Clone()
        {
            Customer clone = new(
                Id,
                Name,
                Email,
                Document,
                Gender,
                Address,
                Username,
                PasswordHash,
                PasswordSalt,
                PasswordSaltIterations,
                IsBoundedTo,
                Phones,
                Sales,
                BoundedCustomers,
                OriginalSales
                );
            return clone;
        }
    }
}