using KadoshDomain.Entities;
using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.UserQueries.DTOs;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

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
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_ALL_USERS_QUERY);
                return new GetAllUsersQueryResult(errors);
            }

            IEnumerable<User> users;

            if (command.PageSize == 0 || command.CurrentPage == 0)
                users = await _userRepository.ReadAllAsync();
            else
                users = await _userRepository.ReadAllPagedAsync(command.CurrentPage, command.PageSize);

            HashSet<UserDTO> usersDTO = new();

            foreach (var user in users)
            {
                usersDTO.Add(user);
            }

            GetAllUsersQueryResult result = new()
            {
                Users = usersDTO
            };
            result.UsersCount = await _userRepository.CountAllAsync();

            return result;
        }
    }
}
