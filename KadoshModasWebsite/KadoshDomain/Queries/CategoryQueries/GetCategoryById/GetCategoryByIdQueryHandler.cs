using KadoshDomain.Queries.Base;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Constants.ServicesMessages;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.CategoryQueries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : QueryHandlerBase<GetCategoryByIdQuery, GetCategoryByIdQueryResult>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public override async Task<GetCategoryByIdQueryResult> HandleAsync(GetCategoryByIdQuery command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_CATEGORY_BY_ID_QUERY);
                return new GetCategoryByIdQueryResult(errors);
            }

            var category = await _categoryRepository.ReadAsync(command.CategoryId!.Value);

            if (category is null)
            {
                AddNotification(nameof(category), CategoryServiceMessages.ERROR_CATEGORY_ID_NOT_FOUND);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_BRAND_NOT_FOUND);
                return new GetCategoryByIdQueryResult(errors);
            }

            GetCategoryByIdQueryResult result = new()
            {
                Category = category
            };

            return result;
        }
    }
}
