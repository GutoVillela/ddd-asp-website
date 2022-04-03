
using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.SaleQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.SaleQueries.GetAllOpenSales
{
    public class GetAllOpenSalesQueryResult : QueryResultBase
    {
        public GetAllOpenSalesQueryResult() { }

        public GetAllOpenSalesQueryResult(IEnumerable<Error> errors) : base(errors) { }

        public IEnumerable<SaleBaseDTO> OpenSales { get; set; } = new List<SaleBaseDTO>();

        public int OpenSalesCount { get; set; }
    }
}
