using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.StoreQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.StoreQueries.GetStoreById
{
    public class GetStoreByIdQueryResult : QueryResultBase
    {
        public GetStoreByIdQueryResult() { }

        public GetStoreByIdQueryResult(IEnumerable<Error> errors)
        {
            Errors = errors as IReadOnlyCollection<Error>;
        }

        public StoreDTO? Store { get; set; }
    }
}
