using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CategoryQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.CategoryQueries.GetAllCategories
{
    public class GetAllCategoriesQueryResult : QueryResultBase
    {
        public GetAllCategoriesQueryResult() { }

        public GetAllCategoriesQueryResult(IEnumerable<Error> errors) : base(errors) { }

        public IEnumerable<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();

        public int CategoriesCount { get; set; }
    }
}
