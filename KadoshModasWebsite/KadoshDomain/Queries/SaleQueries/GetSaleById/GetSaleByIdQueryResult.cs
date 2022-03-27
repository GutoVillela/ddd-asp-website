using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.SaleQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.SaleQueries.GetSaleById
{
    public class GetSaleByIdQueryResult : QueryResultBase
    {
        public GetSaleByIdQueryResult() { }

        public GetSaleByIdQueryResult(IEnumerable<Error> errors) : base(errors) { }

        public SaleBaseDTO? Sale { get; set; }
    }
}
