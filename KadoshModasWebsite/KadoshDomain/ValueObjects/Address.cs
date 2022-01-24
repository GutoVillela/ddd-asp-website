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

        [MaxLength(255)]
        public string Street { get; private set; }

        [MaxLength(20)]
        public string Number { get; private set; }

        [MaxLength(255)]
        public string Neighborhood { get; private set; }

        [MaxLength(50)]
        public string City { get; private set; }

        [MaxLength(20)]
        public string State { get; private set; }

        [MaxLength(10)]
        public string ZipCode { get; private set; }

        [MaxLength(255)]
        public string Complement { get; private set; }

        public override int GetHashCode()
        {
            return  Street.GetHashCode()
                    + Number.GetHashCode()
                    + Neighborhood.GetHashCode()
                    + City.GetHashCode()
                    + State.GetHashCode()
                    + ZipCode.GetHashCode()
                    + Complement.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Address other) return false;

            return Street.Equals(other.Street)
                    && Number.Equals(other.Number)
                    && Neighborhood.Equals(other.Neighborhood)
                    && City.Equals(other.City)
                    && State.Equals(other.State)
                    && ZipCode.Equals(other.ZipCode)
                    && Complement.Equals(other.Complement);
        }
    }
}
