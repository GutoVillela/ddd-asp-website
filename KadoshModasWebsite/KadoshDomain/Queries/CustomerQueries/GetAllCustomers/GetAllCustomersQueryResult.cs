using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CustomerQueries.DTOs;

namespace KadoshDomain.Queries.CustomerQueries.GetAllCustomers
{
    public class GetAllCustomersQueryResult : QueryResultBase
    {
        public IEnumerable<CustomerDTO> Customers { get; set; } = new List<CustomerDTO>();
    }
}
