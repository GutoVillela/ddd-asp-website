using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshShared.Commands;

namespace KadoshDomain.Services.Interfaces
{
    public interface IUserService
    {
        Task<ICommandResult> CreateUserAsync(CreateUserCommand command);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserAsync(int id);
        Task<ICommandResult> UpdateUserAsync(UpdateUserCommand command);
        Task<ICommandResult> DeleteUserAsync(DeleteUserCommand command);
    }
}
