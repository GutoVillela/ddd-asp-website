using KadoshShared.Queries;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.Base
{
    public class QueryResultBase : IQueryResult
    {
        public IReadOnlyCollection<Error>? Errors { get; protected set; }
        public bool Success { get => Errors?.Any() ?? true; }
    }
}
