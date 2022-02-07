using KadoshDomain.Commands;
using KadoshDomain.Services.Interfaces;
using KadoshShared.Commands;
using KadoshShared.Constants.ServicesMessages;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class CategoryApplicationService : ICategoryApplicationService
    {
        private readonly ICategoryService _categoryService;

        public CategoryApplicationService(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<ICommandResult> CreateCategoryAsync(CategoryViewModel Category)
        {
            CreateCategoryCommand command = new();
            command.Name = Category.Name;

            return await _categoryService.CreateCategoryAsync(command);
        }

        public async Task<ICommandResult> DeleteCategoryAsync(int id)
        {
            DeleteCategoryCommand command = new();
            command.Id = id;

            return await _categoryService.DeleteCategoryAsync(command);
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            List<CategoryViewModel> categoriesViewModel = new();

            foreach(var category in categories)
            {
                categoriesViewModel.Add(new CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }

            return categoriesViewModel;
        }

        public async Task<CategoryViewModel> GetCategoryAsync(int id)
        {
            var Category = await _categoryService.GetCategoryAsync(id);
            
            if(Category is null)
                throw new ApplicationException(CategoryServiceMessages.ERROR_CATEGORY_ID_NOT_FOUND);

            CategoryViewModel CategoryViewModel = new()
            {
                Id = Category.Id,
                Name = Category.Name
            };

            return CategoryViewModel;
        }

        public async Task<ICommandResult> UpdateCategoryAsync(CategoryViewModel Category)
        {
            UpdateCategoryCommand command = new();
            command.Id = Category.Id;
            command.Name = Category.Name;

            return await _categoryService.UpdateCategoryAsync(command);
        }
    }
}
