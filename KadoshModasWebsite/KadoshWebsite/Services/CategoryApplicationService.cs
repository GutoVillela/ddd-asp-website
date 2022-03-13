using KadoshDomain.Commands.CategoryCommands.CreateCategory;
using KadoshDomain.Commands.CategoryCommands.DeleteCategory;
using KadoshDomain.Commands.CategoryCommands.UpdateCategory;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.ServicesMessages;
using KadoshShared.Handlers;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class CategoryApplicationService : ICategoryApplicationService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICommandHandler<CreateCategoryCommand> _createCategoryHandler;
        private readonly ICommandHandler<DeleteCategoryCommand> _deleteCategoryHandler;
        private readonly ICommandHandler<UpdateCategoryCommand> _updateCategoryHandler;

        public CategoryApplicationService(
            ICategoryRepository categoryRepository,
            ICommandHandler<CreateCategoryCommand> createCategoryHandler,
            ICommandHandler<DeleteCategoryCommand> deleteCategoryHandler,
            ICommandHandler<UpdateCategoryCommand> updateCategoryHandler)
        {
            _categoryRepository = categoryRepository;
            _createCategoryHandler = createCategoryHandler;
            _deleteCategoryHandler = deleteCategoryHandler;
            _updateCategoryHandler = updateCategoryHandler;
        }

        public async Task<ICommandResult> CreateCategoryAsync(CategoryViewModel Category)
        {
            CreateCategoryCommand command = new();
            command.Name = Category.Name;

            return await _createCategoryHandler.HandleAsync(command);
        }

        public async Task<ICommandResult> DeleteCategoryAsync(int id)
        {
            DeleteCategoryCommand command = new();
            command.Id = id;

            return await _deleteCategoryHandler.HandleAsync(command);
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.ReadAllAsync();
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
            var Category = await _categoryRepository.ReadAsync(id);
            
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

            return await _updateCategoryHandler.HandleAsync(command);
        }
    }
}
