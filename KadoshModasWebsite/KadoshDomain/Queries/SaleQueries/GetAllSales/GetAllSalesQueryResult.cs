using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.SaleQueries.DTOs;

namespace KadoshDomain.Queries.SaleQueries.GetAllSales
{
    public class GetAllSalesQueryResult : QueryResultBase
    {
        public IEnumerable<SaleBaseDTO> Sales { get; set; } = new List<SaleBaseDTO>();
    }
}
