using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CustomerQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.CustomerQueries.GetCustomerByUsername
{
    public class GetCustomerByUsernameQueryResult : QueryResultBase
    {
        public GetCustomerByUsernameQueryResult() { }

        public GetCustomerByUsernameQueryResult(IEnumerable<Error> errors)
        {
            Errors = errors as IReadOnlyCollection<Error>;
        }

        public CustomerDTO? Customer { get; set; }
    }
}
