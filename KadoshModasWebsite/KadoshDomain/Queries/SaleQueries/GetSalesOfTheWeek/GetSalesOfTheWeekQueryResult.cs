using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.SaleQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.SaleQueries.GetSalesOfTheWeek
{
    public class GetSalesOfTheWeekQueryResult : QueryResultBase
    {
        public GetSalesOfTheWeekQueryResult() { }

        public GetSalesOfTheWeekQueryResult(IEnumerable<Error> errors) : base(errors) { }

        public IEnumerable<SaleBaseDTO> SalesOfTheWeek { get; set; } = new List<SaleBaseDTO>();

        public int SalesOfTheWeekCount { get; set; }

        public decimal TotalAmountToPayFromSalesOfTheWeek { get; set; }

        public decimal TotalAmountPaidFromSalesOfTheWeek { get; set; }

        public decimal TotalAmountFromSalesOfTheWeek { get; set; }
    }
}
