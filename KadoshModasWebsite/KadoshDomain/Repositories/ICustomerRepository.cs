using KadoshDomain.Entities;

namespace KadoshDomain.Repositories
{
    public interface ICustomerRepository
    {
        void CreateCustomer(Customer customer);
    }
}
