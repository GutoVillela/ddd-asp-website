using KadoshDomain.Queries.Base;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.CustomerQueries.GetCustomerTotalDebt
{
    public class GetCustomerTotalDebtQueryResult : QueryResultBase
    {
        public GetCustomerTotalDebtQueryResult() { }

        public GetCustomerTotalDebtQueryResult(IEnumerable<Error> errors)
        {
            Errors = errors as IReadOnlyCollection<Error>;
        }

        public decimal CustomerTotalDebt { get; set; }
    }
}
