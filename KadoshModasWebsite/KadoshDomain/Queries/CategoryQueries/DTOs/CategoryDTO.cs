using KadoshDomain.Entities;

namespace KadoshDomain.Queries.CategoryQueries.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static implicit operator CategoryDTO(Category category) => new() { Id = category.Id, Name = category.Name };
    }
}
