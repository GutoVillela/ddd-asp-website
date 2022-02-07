using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshShared.Commands;

namespace KadoshDomain.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ICommandResult> CreateCategoryAsync(CreateCategoryCommand command);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryAsync(int id);
        Task<ICommandResult> UpdateCategoryAsync(UpdateCategoryCommand command);
        Task<ICommandResult> DeleteCategoryAsync(DeleteCategoryCommand command);
    }
}
