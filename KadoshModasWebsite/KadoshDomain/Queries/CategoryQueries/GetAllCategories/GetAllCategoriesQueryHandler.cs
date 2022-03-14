using KadoshDomain.Entities;
using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CategoryQueries.DTOs;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

namespace KadoshDomain.Queries.CategoryQueries.GetAllCategories
{
    public class GetAllCategoriesQueryHandler : QueryHandlerBase<GetAllCategoriesQuery, GetAllCategoriesQueryResult>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public override async Task<GetAllCategoriesQueryResult> HandleAsync(GetAllCategoriesQuery command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_ALL_CATEGORIES_QUERY);
                return new GetAllCategoriesQueryResult(errors);
            }

            IEnumerable<Category> categories;


            if (command.PageSize == 0 || command.CurrentPage == 0)
                categories = await _categoryRepository.ReadAllAsync();
            else
                categories = await _categoryRepository.ReadAllPagedAsync(command.CurrentPage, command.PageSize);

            HashSet<CategoryDTO> categoriesDTO = new();

            foreach (var category in categories)
            {
                categoriesDTO.Add(category);
            }

            GetAllCategoriesQueryResult result = new()
            {
                Categories = categoriesDTO
            };
            result.CategoriesCount = await _categoryRepository.CountAllAsync();

            return result;
        }
    }
}