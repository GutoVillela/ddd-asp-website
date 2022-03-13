using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CustomerQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.CustomerQueries.GetCustomerById
{
    public class GetCustomerByIdQueryResult : QueryResultBase
    {
        public GetCustomerByIdQueryResult() { }

        public GetCustomerByIdQueryResult(IEnumerable<Error> errors)
        {
            Errors = errors as IReadOnlyCollection<Error>;
        }

        public CustomerDTO? Customer { get; set; }
    }
}
