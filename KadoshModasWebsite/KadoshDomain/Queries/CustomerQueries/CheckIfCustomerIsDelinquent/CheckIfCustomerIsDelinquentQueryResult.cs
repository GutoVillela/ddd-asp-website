using KadoshDomain.Queries.Base;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.CustomerQueries.CheckIfCustomerIsDelinquent
{
    public class CheckIfCustomerIsDelinquentQueryResult : QueryResultBase
    {
        public CheckIfCustomerIsDelinquentQueryResult() { }

        public CheckIfCustomerIsDelinquentQueryResult(IEnumerable<Error> errors)
        {
            Errors = errors as IReadOnlyCollection<Error>;
        }

        public bool IsDelinquent { get; set; }
    }
}
