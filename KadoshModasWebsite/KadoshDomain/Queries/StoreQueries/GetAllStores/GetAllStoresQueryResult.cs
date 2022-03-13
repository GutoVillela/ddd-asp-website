using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.StoreQueries.DTOs;

namespace KadoshDomain.Queries.StoreQueries.GetAllStores
{
    public class GetAllStoresQueryResult : QueryResultBase
    {
        public IEnumerable<StoreDTO> Stores { get; set; } = new List<StoreDTO>();
    }
}
