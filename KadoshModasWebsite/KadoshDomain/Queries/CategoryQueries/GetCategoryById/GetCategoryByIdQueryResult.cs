using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CategoryQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.CategoryQueries.GetCategoryById
{
    public class GetCategoryByIdQueryResult : QueryResultBase
    {
        public GetCategoryByIdQueryResult() { }

        public GetCategoryByIdQueryResult(IEnumerable<Error> errors)
        {
            Errors = errors as IReadOnlyCollection<Error>;
        }

        public CategoryDTO? Category { get; set; }
    }
}
