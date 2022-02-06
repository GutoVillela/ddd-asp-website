using KadoshDomain.Commands;
using KadoshDomain.Services.Interfaces;
using KadoshShared.Commands;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using KadoshDomain.Enums;
using KadoshDomain.ValueObjects;
using KadoshShared.Constants.ServicesMessages;
using KadoshDomain.Entities;
using KadoshWebsite.Util;

namespace KadoshWebsite.Services
{
    public class UserApplicationService : IUserApplicationService
    {
        private readonly IUserService _userService;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession? _session => _httpContextAccessor.HttpContext?.Session;

        public UserApplicationService(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ICommandResult> CreateUserAsync(UserViewModel user)
        {
            CreateUserCommand command = new();
            command.Name = user.Name;
            command.Username = user.UserName;
            command.Password = user.Password;
            command.Role = user.Role;
            command.StoreId = user.StoreId;

            return await _userService.CreateUserAsync(command);
        }

        public async Task<ICommandResult> DeleteUserAsync(int id)
        {
            DeleteUserCommand command = new();
            command.Id = id;

            return await _userService.DeleteUserAsync(command);
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
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
            var user = await _userService.GetUserAsync(id);
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

            return await _userService.UpdateUserAsync(command);
        }

        public async Task<ICommandResult> AuthenticateUserAsync(string username, string password)
        {
            AuthenticateUserCommand command = new();
            command.Username = username;
            command.Password = password;

            var result = await _userService.AuthenticateUserAsync(command);
            return result;
        }

        public void LoginAuthenticatedUser(string authenticatedUsername)
        {
            if (_session is null)
                throw new ApplicationException("Não existe sessão habilitada para validar usuário");

            _session.SetString(AuthorizationConstants.LOGGED_IN_USERNAME_SESSION, authenticatedUsername);
        }
    }
}