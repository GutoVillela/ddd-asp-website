using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.UserQueries.DTOs;

namespace KadoshDomain.Queries.UserQueries.GetAllUsers
{
    public class GetAllUsersQueryResult : QueryResultBase
    {
        public IEnumerable<UserDTO> Users { get; set; } = new List<UserDTO>();
    }
}
