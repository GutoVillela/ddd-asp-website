using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CustomerPostingQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.CustomerPostingQueries.GetAllPostingsFromDate
{
    public class GetAllPostingsFromStoreAndDateQueryResult : QueryResultBase
    {
        public GetAllPostingsFromStoreAndDateQueryResult() { }

        public GetAllPostingsFromStoreAndDateQueryResult(IEnumerable<Error> errors) : base(errors) { }

        public IEnumerable<CustomerPostingDTO> CustomerPostings { get; set; } = new List<CustomerPostingDTO>();

        public int CustomerPostingsCount { get; set; }

        public decimal TotalCredit { get; set; }

        public decimal TotalDebit { get; set; }
    }
}
