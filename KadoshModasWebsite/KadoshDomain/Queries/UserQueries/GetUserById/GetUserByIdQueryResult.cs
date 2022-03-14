using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.UserQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.UserQueries.GetUserById
{
    public class GetUserByIdQueryResult : QueryResultBase
    {
        public GetUserByIdQueryResult() { }

        public GetUserByIdQueryResult(IEnumerable<Error> errors)
        {
            Errors = errors as IReadOnlyCollection<Error>;
        }

        public UserDTO? User { get; set; }
    }
}
