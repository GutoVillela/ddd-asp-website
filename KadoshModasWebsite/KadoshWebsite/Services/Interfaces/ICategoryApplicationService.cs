﻿using KadoshShared.Commands;
using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface ICategoryApplicationService
    {
        Task<ICommandResult> CreateCategoryAsync(CategoryViewModel category);
        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();
        Task<CategoryViewModel> GetCategoryAsync(int id);
        Task<ICommandResult> UpdateCategoryAsync(CategoryViewModel category);
        Task<ICommandResult> DeleteCategoryAsync(int id);
    }
}
