using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.StoreQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.StoreQueries.GetAllStores
{
    public class GetAllStoresQueryResult : QueryResultBase
    {
        public GetAllStoresQueryResult() { }

        public GetAllStoresQueryResult(IEnumerable<Error> errors) : base(errors) { }

        public IEnumerable<StoreDTO> Stores { get; set; } = new List<StoreDTO>();

        public int StoresCount { get; set; }
    }
}
