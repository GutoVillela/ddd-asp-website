using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CategoryQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.CategoryQueries.GetCategoryById
{
    public class GetCategoryByIdQueryResult : QueryResultBase
    {
        public GetCategoryByIdQueryResult() { }

        public GetCategoryByIdQueryResult(IReadOnlyCollection<Error> errors)
        {
            Errors = errors;
        }

        public CategoryDTO? Category { get; set; }
    }
}
