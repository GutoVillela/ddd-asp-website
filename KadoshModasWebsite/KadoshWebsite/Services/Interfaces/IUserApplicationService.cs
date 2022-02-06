using KadoshShared.Commands;
using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface IUserApplicationService
    {
        Task<ICommandResult> CreateUserAsync(UserViewModel user);
        Task<IEnumerable<UserViewModel>> GetAllUsersAsync();
        Task<UserViewModel> GetUserAsync(int id);
        Task<ICommandResult> UpdateUserAsync(UserViewModel user);
        Task<ICommandResult> DeleteUserAsync(int id);
        Task<ICommandResult> AuthenticateUserAsync(string username, string password);
    }
}
