using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.SaleQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.SaleQueries.GetSalesByDate
{
    public class GetSalesByDateQueryResult : QueryResultBase
    {
        public GetSalesByDateQueryResult() { }

        public GetSalesByDateQueryResult(IEnumerable<Error> errors) : base(errors) { }

        public IEnumerable<SaleBaseDTO> Sales { get; set; } = new List<SaleBaseDTO>();

        public int SalesCount { get; set; }

        public decimal TotalAmountToPayFromSales { get; set; }

        public decimal TotalAmountPaidFromSales { get; set; }

        public decimal TotalAmountFromSales { get; set; }
    }
}
