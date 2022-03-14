using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.UserQueries.DTOs;
using KadoshDomain.Repositories;

namespace KadoshDomain.Queries.UserQueries.GetAllUsers
{
    public class GetAllUsersQueryHandler : QueryHandlerBase<GetAllUsersQuery, GetAllUsersQueryResult>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public override async Task<GetAllUsersQueryResult> HandleAsync(GetAllUsersQuery command)
        {
            var users = await _userRepository.ReadAllAsync();
            HashSet<UserDTO> usersDTO = new();

            foreach (var user in users)
            {
                usersDTO.Add(user);
            }

            GetAllUsersQueryResult result = new()
            {
                Users = usersDTO
            };
            return result;
        }
    }
}
