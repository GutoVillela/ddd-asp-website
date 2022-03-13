using KadoshShared.Commands;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using KadoshShared.Constants.ServicesMessages;
using KadoshWebsite.Util;
using KadoshDomain.Repositories;
using KadoshDomain.Commands.UserCommands.CreateUser;
using KadoshDomain.Commands.UserCommands.DeleteUser;
using KadoshDomain.Commands.UserCommands.UpdateUser;
using KadoshShared.Handlers;
using KadoshDomain.Commands.UserCommands.AuthenticateUser;

namespace KadoshWebsite.Services
{
    public class UserApplicationService : IUserApplicationService
    {
        private readonly IUserRepository _userRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession? _session => _httpContextAccessor.HttpContext?.Session;

        private readonly ICommandHandler<AuthenticateUserCommand> _authenticateUserHandler;
        private readonly ICommandHandler<CreateUserCommand> _createUserHandler;
        private readonly ICommandHandler<DeleteUserCommand> _deleteUserHandler;
        private readonly ICommandHandler<UpdateUserCommand> _updateUserHandler;

        public UserApplicationService(
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            ICommandHandler<AuthenticateUserCommand> authenticateUserHandler,
            ICommandHandler<CreateUserCommand> createUserHandler,
            ICommandHandler<DeleteUserCommand> deleteUserHandler,
            ICommandHandler<UpdateUserCommand> updateUserHandler
            )
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _authenticateUserHandler = authenticateUserHandler;
            _createUserHandler = createUserHandler;
            _deleteUserHandler = deleteUserHandler;
            _updateUserHandler = updateUserHandler;
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
            var users = await _userRepository.ReadAllAsync();
            var usersViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                usersViewModels.Add(new UserViewModel()
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserName = user.Username,
                    UserNameBeforeEdit = user.Username,
                    Role = user.Role,
                    StoreId = user.StoreId
                });
            }
            return usersViewModels;
        }

        public async Task<UserViewModel> GetUserAsync(int id)
        {
            var user = await _userRepository.ReadAsync(id);
            if (user is null)
                throw new ApplicationException(UserServiceMessages.ERROR_USER_NOT_FOUND);

            UserViewModel viewModel = new()
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.Username,
                UserNameBeforeEdit = user.Username,
                Role = user.Role,
                StoreId = user.StoreId
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