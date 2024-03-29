﻿using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshShared.Commands;
using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface IUserApplicationService
    {
        Task<ICommandResult> CreateUserAsync(UserViewModel user);
        Task<IEnumerable<UserViewModel>> GetAllUsersAsync();
        Task<PaginatedListViewModel<UserViewModel>> GetAllUsersPaginatedAsync(int currentPage, int pageSize);
        Task<UserViewModel> GetUserAsync(int id);
        Task<ICommandResult> UpdateUserAsync(UserViewModel user);
        Task<ICommandResult> DeleteUserAsync(int id);
        Task<ICommandResult> AuthenticateUserAsync(string username, string password);
        void LoginAuthenticatedUser(string authenticatedUsername);
        Task<ICommandResult> AuthenticateCustomerUserAsync(string username, string password);
    }
}
