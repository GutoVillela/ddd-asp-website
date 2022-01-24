using KadoshDomain.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KadoshTests.ValueObjects
{
    [TestClass]
    public class AddressTests
    {
        private readonly Address _address;
        private readonly Address _addressEquals;
        private readonly Address _addressDifferent;

        public AddressTests()
        {
            _address = new(
                street: "Street",
                number: "10",
                neighborhood: "Neighborhood",
                city: "City",
                state: "State",
                zipCode: "00001-000",
                complement: "House 2"
                );

            _addressEquals = new(
                street: "Street",
                number: "10",
                neighborhood: "Neighborhood",
                city: "City",
                state: "State",
                zipCode: "00001-000",
                complement: "House 2"
                ); 
            
            _addressDifferent = new(
                street: "Different Street",
                number: "99",
                neighborhood: "Different Neighborhood",
                city: "Different City",
                state: "Different State",
                zipCode: "12345-678",
                complement: "Different Complement"
                );
        }

        [TestMethod]
        public void ShouldReturnTrueWhenAdressesAreEqualsInValue()
        {
            bool equalsAdresses = _address.Equals(_addressEquals);
            Assert.IsTrue(equalsAdresses);
        }

        [TestMethod]
        public void ShouldReturnFalseWhenAdressesAreDifferentInValue()
        {
            bool equalsAdresses = _address.Equals(_addressDifferent);
            Assert.IsFalse(equalsAdresses);
        }
    }
}
