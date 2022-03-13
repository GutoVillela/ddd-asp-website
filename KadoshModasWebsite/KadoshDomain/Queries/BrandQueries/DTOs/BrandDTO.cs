using KadoshDomain.Entities;

namespace KadoshDomain.Queries.BrandQueries.DTOs
{
    public class BrandDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static implicit operator BrandDTO(Brand brand) => new() { Id = brand.Id, Name = brand.Name };
    }
}
