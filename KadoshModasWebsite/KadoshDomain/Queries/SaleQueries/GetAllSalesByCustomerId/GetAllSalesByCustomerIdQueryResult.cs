using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.SaleQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.SaleQueries.GetAllSalesByCustomerId
{
    public class GetAllSalesByCustomerIdQueryResult : QueryResultBase
    {
        public GetAllSalesByCustomerIdQueryResult() { }

        public GetAllSalesByCustomerIdQueryResult(IEnumerable<Error> errors)
        {
            Errors = errors as IReadOnlyCollection<Error>;
        }

        public IEnumerable<SaleBaseDTO> Sales { get; set; } = new List<SaleBaseDTO>();
    }
}
