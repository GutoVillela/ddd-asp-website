using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CustomerQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.CustomerQueries.GetAllDelinquentCustomers
{
    public  class GetAllDelinquentCustomersQueryResult : QueryResultBase
    {
        public GetAllDelinquentCustomersQueryResult() { }

        public GetAllDelinquentCustomersQueryResult(IEnumerable<Error> errors) : base(errors) { }

        public IEnumerable<CustomerDTO> DelinquentCustomers { get; set; } = new List<CustomerDTO>();

        public int DelinquentCustomersCount { get; set; }
    }
}
