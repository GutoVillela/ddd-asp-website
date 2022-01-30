using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Handlers;
using KadoshDomain.Repositories;
using KadoshDomain.Services.Interfaces;
using KadoshShared.Commands;

namespace KadoshDomain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ICommandResult> CreateUserAsync(CreateUserCommand command)
        {
            UserHandler userHandler = new(_userRepository);
            return await userHandler.HandleAsync(command);
        }

        public async Task<ICommandResult> DeleteUserAsync(DeleteUserCommand command)
        {
            UserHandler userHandler = new(_userRepository);
            return await userHandler.HandleAsync(command);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.ReadAllAsync();
        }

        public async Task<User?> GetUserAsync(int id)
        {
            return await _userRepository.ReadAsync(id);
        }

        public async Task<ICommandResult> UpdateUserAsync(UpdateUserCommand command)
        {
            UserHandler userHandler = new(_userRepository);
            return await userHandler.HandleAsync(command);
        }
    }
}
