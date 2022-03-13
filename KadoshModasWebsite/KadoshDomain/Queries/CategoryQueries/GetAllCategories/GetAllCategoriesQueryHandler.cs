using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CategoryQueries.DTOs;
using KadoshDomain.Repositories;

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
            var categories = await _categoryRepository.ReadAllAsync();
            HashSet<CategoryDTO> categoriesDTO = new();

            foreach (var category in categories)
            {
                categoriesDTO.Add(category);
            }

            GetAllCategoriesQueryResult result = new()
            {
                Categories = categoriesDTO
            };
            return result;
        }
    }
}
