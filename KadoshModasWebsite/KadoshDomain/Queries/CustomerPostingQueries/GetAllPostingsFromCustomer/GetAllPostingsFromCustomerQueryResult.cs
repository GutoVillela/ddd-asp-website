using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CustomerPostingQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.CustomerPostingQueries.GetAllPostingsFromCustomer
{
    public class GetAllPostingsFromCustomerQueryResult : QueryResultBase
    {
        public GetAllPostingsFromCustomerQueryResult() { }

        public GetAllPostingsFromCustomerQueryResult(IEnumerable<Error> errors)
        {
            Errors = errors as IReadOnlyCollection<Error>;
        }

        public IEnumerable<CustomerPostingDTO> CustomerPostings { get; set; } = new List<CustomerPostingDTO>();
    }
}
