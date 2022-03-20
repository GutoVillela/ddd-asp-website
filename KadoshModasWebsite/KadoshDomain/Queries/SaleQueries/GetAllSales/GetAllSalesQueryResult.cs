using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.SaleQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.SaleQueries.GetAllSales
{
    public class GetAllSalesQueryResult : QueryResultBase
    {
        public GetAllSalesQueryResult() { }

        public GetAllSalesQueryResult(IEnumerable<Error> errors) : base(errors) { }

        public IEnumerable<SaleBaseDTO> Sales { get; set; } = new List<SaleBaseDTO>();

        public int SalesCount { get; set; }
    }
}
