using KadoshDomain.Commands;
using KadoshDomain.Handlers;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.ServicesMessages;
using KadoshShared.Repositories;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class CategoryApplicationService : ICategoryApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryApplicationService(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
        }

        public async Task<ICommandResult> CreateCategoryAsync(CategoryViewModel Category)
        {
            CreateCategoryCommand command = new();
            command.Name = Category.Name;

            CategoryHandler categoryHandler = new(_unitOfWork, _categoryRepository);
            return await categoryHandler.HandleAsync(command);
        }

        public async Task<ICommandResult> DeleteCategoryAsync(int id)
        {
            DeleteCategoryCommand command = new();
            command.Id = id;

            CategoryHandler categoryHandler = new(_unitOfWork, _categoryRepository);
            return await categoryHandler.HandleAsync(command);
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

            CategoryHandler categoryHandler = new(_unitOfWork, _categoryRepository);
            return await categoryHandler.HandleAsync(command);
        }
    }
}
