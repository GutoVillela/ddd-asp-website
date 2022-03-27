using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CustomerPostingQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.CustomerPostingQueries.GetAllPostingsFromSale
{
    public class GetAllPostingsFromSaleQueryResult : QueryResultBase
    {
        public GetAllPostingsFromSaleQueryResult() { }

        public GetAllPostingsFromSaleQueryResult(IEnumerable<Error> errors) : base(errors) { }

        public IEnumerable<CustomerPostingDTO> CustomerPostings { get; set; } = new List<CustomerPostingDTO>();

        public int CustomerPostingsCount { get; set; }
    }
}
