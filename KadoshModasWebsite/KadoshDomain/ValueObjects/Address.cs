using KadoshShared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.ValueObjects
{
    public class Address : ValueObject
    {
        public Address(string street, string number, string neighborhood, string city, string state, string zipCode, string complement)
        {
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            ZipCode = zipCode;
            Complement = complement;
        }

        [Required]
        [MaxLength(255)]
        public string Street { get; private set; }

        [Required]
        [MaxLength(20)]
        public string Number { get; private set; }

        [Required]
        [MaxLength(255)]
        public string Neighborhood { get; private set; }

        [Required]
        [MaxLength(50)]
        public string City { get; private set; }

        [Required]
        [MaxLength(20)]
        public string State { get; private set; }

        [Required]
        [MaxLength(10)]
        public string ZipCode { get; private set; }

        [Required]
        [MaxLength(255)]
        public string Complement { get; private set; }
    }
}
