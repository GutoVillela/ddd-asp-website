using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.ValueObjects;
using KadoshShared.Entities;

namespace KadoshDomain.LegacyEntities
{
    /// <summary>
    /// Entity class for Customer in Legacy Database.
    /// </summary>
    public class CustomerLegacy : LegacyEntity<Customer>
    {
        public CustomerLegacy(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public Email? Email { get; set; }
        public Document? Document { get; set; }
        public EGender? Gender { get; set; }
        public Address? Address { get; set; }
        public IReadOnlyCollection<Phone>? Phones { get; private set; }

        public void SetPhones(ICollection<Phone> phones)
        {
            Phones = phones as IReadOnlyCollection<Phone>;
        }

        public static implicit operator Customer(CustomerLegacy legacyCustomer)
        {
            Customer customer = new(
                name: legacyCustomer.Name,
                email: legacyCustomer.Email,
                document: legacyCustomer.Document,
                gender: legacyCustomer.Gender,
                address: legacyCustomer.Address,
                phones: legacyCustomer.Phones);

            if (!legacyCustomer.IsActive)
                customer.Inactivate();

            return customer;
        }

    }
}
