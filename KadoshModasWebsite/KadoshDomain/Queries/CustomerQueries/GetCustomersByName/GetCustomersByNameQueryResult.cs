using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CustomerQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.CustomerQueries.GetCustomersByName
{
    public class GetCustomersByNameQueryResult : QueryResultBase
    {
        public GetCustomersByNameQueryResult() { }

        public GetCustomersByNameQueryResult(IEnumerable<Error> errors) : base(errors) { }

        public IEnumerable<CustomerDTO> Customers { get; set; } = new List<CustomerDTO>();

        public int CustomersCount { get; set; }
    }
}
