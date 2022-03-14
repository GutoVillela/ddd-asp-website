using KadoshDomain.Queries.Base;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Constants.QueriesMessages;

namespace KadoshDomain.Queries.UserQueries.GetUserById
{
    public class GetUserByIdQueryHandler : QueryHandlerBase<GetUserByIdQuery, GetUserByIdQueryResult>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public override async Task<GetUserByIdQueryResult> HandleAsync(GetUserByIdQuery command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_USER_BY_ID_QUERY);
                return new GetUserByIdQueryResult(errors);
            }

            var user = await _userRepository.ReadAsync(command.UserId!.Value);

            if (user is null)
            {
                AddNotification(nameof(user), UserQueriesMessages.ERROR_USER_NOT_FOUND);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_USERNAME_NOT_FOUND);
                return new GetUserByIdQueryResult(errors);
            }

            GetUserByIdQueryResult result = new()
            {
                User = user
            };

            return result;
        }
    }
}
