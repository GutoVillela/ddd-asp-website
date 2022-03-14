using KadoshShared.Commands;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using KadoshWebsite.Util;
using KadoshDomain.Commands.UserCommands.CreateUser;
using KadoshDomain.Commands.UserCommands.DeleteUser;
using KadoshDomain.Commands.UserCommands.UpdateUser;
using KadoshShared.Handlers;
using KadoshDomain.Commands.UserCommands.AuthenticateUser;
using KadoshDomain.Queries.UserQueries.GetAllUsers;
using KadoshDomain.Queries.UserQueries.GetUserById;
using KadoshShared.ExtensionMethods;

namespace KadoshWebsite.Services
{
    public class UserApplicationService : IUserApplicationService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession? _session => _httpContextAccessor.HttpContext?.Session;

        private readonly ICommandHandler<AuthenticateUserCommand> _authenticateUserHandler;
        private readonly ICommandHandler<CreateUserCommand> _createUserHandler;
        private readonly ICommandHandler<DeleteUserCommand> _deleteUserHandler;
        private readonly ICommandHandler<UpdateUserCommand> _updateUserHandler;

        private readonly IQueryHandler<GetAllUsersQuery, GetAllUsersQueryResult> _getAllUsersQueryHandler;
        private readonly IQueryHandler<GetUserByIdQuery, GetUserByIdQueryResult> _getUserByIdQueryHandler;

        public UserApplicationService(
            IHttpContextAccessor httpContextAccessor,
            ICommandHandler<AuthenticateUserCommand> authenticateUserHandler,
            ICommandHandler<CreateUserCommand> createUserHandler,
            ICommandHandler<DeleteUserCommand> deleteUserHandler,
            ICommandHandler<UpdateUserCommand> updateUserHandler,
            IQueryHandler<GetAllUsersQuery, GetAllUsersQueryResult> getAllUsersQueryHandler,
            IQueryHandler<GetUserByIdQuery, GetUserByIdQueryResult> getUserByIdQueryHandler
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _authenticateUserHandler = authenticateUserHandler;
            _createUserHandler = createUserHandler;
            _deleteUserHandler = deleteUserHandler;
            _updateUserHandler = updateUserHandler;
            _getAllUsersQueryHandler = getAllUsersQueryHandler;
            _getUserByIdQueryHandler = getUserByIdQueryHandler;
        }

        public async Task<ICommandResult> CreateUserAsync(UserViewModel user)
        {
            CreateUserCommand command = new();
            command.Name = user.Name;
            command.Username = user.UserName;
            command.Password = user.Password;
            command.Role = user.Role;
            command.StoreId = user.StoreId;

            return await _createUserHandler.HandleAsync(command);
        }

        public async Task<ICommandResult> DeleteUserAsync(int id)
        {
            DeleteUserCommand command = new();
            command.Id = id;

            return await _deleteUserHandler.HandleAsync(command);
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
        {
            var result = await _getAllUsersQueryHandler.HandleAsync(new GetAllUsersQuery());

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            var usersViewModels = new List<UserViewModel>();

            foreach (var user in result.Users)
            {
                usersViewModels.Add(new UserViewModel()
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserName = user.UserName,
                    UserNameBeforeEdit = user.UserName,
                    Role = user.Role,
                    StoreId = user.StoreId
                });
            }
            return usersViewModels;
        }

        public async Task<UserViewModel> GetUserAsync(int id)
        {
            GetUserByIdQuery query = new();
            query.UserId = id;

            var result = await _getUserByIdQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            UserViewModel viewModel = new()
            {
                Id = result.User!.Id,
                Name = result.User!.Name,
                UserName = result.User!.UserName,
                UserNameBeforeEdit = result.User!.UserName,
                Role = result.User!.Role,
                StoreId = result.User!.StoreId
            };

            return viewModel;
        }

        public async Task<ICommandResult> UpdateUserAsync(UserViewModel user)
        {
            UpdateUserCommand command = new();
            command.Id = user.Id;
            command.Name = user.Name;
            command.NewUsername = user.UserName;
            command.OriginalUsername = user.UserNameBeforeEdit;
            command.Password = user.Password;
            command.Role = user.Role;
            command.StoreId = user.StoreId;

            return await _updateUserHandler.HandleAsync(command);
        }

        public async Task<ICommandResult> AuthenticateUserAsync(string username, string password)
        {
            AuthenticateUserCommand command = new();
            command.Username = username;
            command.Password = password;

            return await _authenticateUserHandler.HandleAsync(command);
        }

        public void LoginAuthenticatedUser(string authenticatedUsername)
        {
            if (_session is null)
                throw new ApplicationException("Não existe sessão habilitada para validar usuário");

            _session.SetString(AuthorizationConstants.LOGGED_IN_USERNAME_SESSION, authenticatedUsername);
        }
    }
}