using KadoshDomain.Commands.CategoryCommands.CreateCategory;
using KadoshDomain.Commands.CategoryCommands.DeleteCategory;
using KadoshDomain.Commands.CategoryCommands.UpdateCategory;
using KadoshDomain.Queries.CategoryQueries.GetAllCategories;
using KadoshDomain.Queries.CategoryQueries.GetCategoryById;
using KadoshShared.Commands;
using KadoshShared.ExtensionMethods;
using KadoshShared.Handlers;
using KadoshWebsite.Infrastructure;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class CategoryApplicationService : ICategoryApplicationService
    {
        private readonly ICommandHandler<CreateCategoryCommand> _createCategoryHandler;
        private readonly ICommandHandler<DeleteCategoryCommand> _deleteCategoryHandler;
        private readonly ICommandHandler<UpdateCategoryCommand> _updateCategoryHandler;
        private readonly IQueryHandler<GetAllCategoriesQuery, GetAllCategoriesQueryResult> _getAllCategoriesQueryHandler;
        private readonly IQueryHandler<GetCategoryByIdQuery, GetCategoryByIdQueryResult> _getCategoryByIdQueryHandler;

        public CategoryApplicationService(
            ICommandHandler<CreateCategoryCommand> createCategoryHandler,
            ICommandHandler<DeleteCategoryCommand> deleteCategoryHandler,
            ICommandHandler<UpdateCategoryCommand> updateCategoryHandler,
            IQueryHandler<GetAllCategoriesQuery, GetAllCategoriesQueryResult> getAllCategoriesQueryHandler,
            IQueryHandler<GetCategoryByIdQuery, GetCategoryByIdQueryResult> getCategoryByIdQueryHandler
            )
        {
            _createCategoryHandler = createCategoryHandler;
            _deleteCategoryHandler = deleteCategoryHandler;
            _updateCategoryHandler = updateCategoryHandler;
            _getAllCategoriesQueryHandler = getAllCategoriesQueryHandler;
            _getCategoryByIdQueryHandler = getCategoryByIdQueryHandler;
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
            var result = await _getAllCategoriesQueryHandler.HandleAsync(new GetAllCategoriesQuery());

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            List<CategoryViewModel> categoriesViewModel = new();

            foreach(var category in result.Categories)
            {
                categoriesViewModel.Add(new CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }

            return categoriesViewModel;
        }

        public async Task<PaginatedListViewModel<CategoryViewModel>> GetAllCategoriesPaginatedAsync(int currentPage, int pageSize)
        {
            GetAllCategoriesQuery query = new();
            query.CurrentPage = currentPage;
            query.PageSize = pageSize;

            var result = await _getAllCategoriesQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            List<CategoryViewModel> categoriesViewModel = new();

            foreach (var category in result.Categories)
            {
                categoriesViewModel.Add(new CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }

            PaginatedListViewModel<CategoryViewModel> paginatedList = new();
            paginatedList.CurrentPage = currentPage;
            paginatedList.PageSize = pageSize;
            paginatedList.TotalItemsCount = result.CategoriesCount;
            paginatedList.TotalPages = PaginationManager.CalculateTotalPages(result.CategoriesCount, pageSize);
            paginatedList.Items = categoriesViewModel;

            return paginatedList;
        }

        public async Task<CategoryViewModel> GetCategoryAsync(int id)
        {
            GetCategoryByIdQuery query = new();
            query.CategoryId = id;

            var result = await _getCategoryByIdQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            CategoryViewModel CategoryViewModel = new()
            {
                Id = result.Category!.Id,
                Name = result.Category!.Name
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