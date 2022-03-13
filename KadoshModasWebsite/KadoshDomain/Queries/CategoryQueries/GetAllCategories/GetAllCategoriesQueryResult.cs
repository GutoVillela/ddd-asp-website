using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CategoryQueries.DTOs;

namespace KadoshDomain.Queries.CategoryQueries.GetAllCategories
{
    public class GetAllCategoriesQueryResult : QueryResultBase
    {
        public IEnumerable<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();
    }
}
