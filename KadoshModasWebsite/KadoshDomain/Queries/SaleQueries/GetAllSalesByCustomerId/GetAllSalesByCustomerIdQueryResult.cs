using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.SaleQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.SaleQueries.GetAllSalesByCustomerId
{
    public class GetAllSalesByCustomerIdQueryResult : QueryResultBase
    {
        public GetAllSalesByCustomerIdQueryResult() { }

        public GetAllSalesByCustomerIdQueryResult(IEnumerable<Error> errors) : base(errors) { }

        public IEnumerable<SaleBaseDTO> Sales { get; set; } = new List<SaleBaseDTO>();

        public int SalesCount { get; set; }
    }
}
