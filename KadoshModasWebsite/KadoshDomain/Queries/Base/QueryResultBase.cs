using KadoshShared.Queries;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.Base
{
    public class QueryResultBase : IQueryResult
    {
        protected QueryResultBase() { }
        protected QueryResultBase(IEnumerable<Error> errors) 
        {
            Errors = errors as IReadOnlyCollection<Error>;
        }

        public IReadOnlyCollection<Error>? Errors { get; protected set; }
        public bool Success { get => !Errors?.Any() ?? true; }
    }
}
