using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.UserQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.UserQueries.GetAllUsers
{
    public class GetAllUsersQueryResult : QueryResultBase
    {
        public GetAllUsersQueryResult() { }

        public GetAllUsersQueryResult(IEnumerable<Error> errors) : base(errors) { }

        public IEnumerable<UserDTO> Users { get; set; } = new List<UserDTO>();

        public int UsersCount { get; set; }
    }
}
