using KadoshDomain.Entities;
using KadoshDomain.ValueObjects;
using System.Linq.Expressions;

namespace KadoshDomain.Queriables
{
    public static class StoreQueriable
    {
        public static Expression<Func<Store, bool>> GetStoreByAddress(Address address)
        {
            return x => x.Address.Street == address.Street 
                        && x.Address.Number == address.Number
                        && x.Address.Neighborhood == address.Neighborhood
                        && x.Address.City == address.City
                        && x.Address.State == address.State
                        && x.Address.ZipCode == address.ZipCode
                        && x.Address.Complement == address.Complement;
        }

        public static Expression<Func<Store, bool>> GetStoreByAddressExceptWithGivenId(Address address, int storeToDisconsider)
        {
            return x => x.Id != storeToDisconsider
                        && x.Address.Street == address.Street
                        && x.Address.Number == address.Number
                        && x.Address.Neighborhood == address.Neighborhood
                        && x.Address.City == address.City
                        && x.Address.State == address.State
                        && x.Address.ZipCode == address.ZipCode
                        && x.Address.Complement == address.Complement;
        }
    }
}
