using KadoshDomain.Entities;

namespace KadoshDomain.Queries.ProductQueries.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? BarCode { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public int BrandId { get; set; }

        public static implicit operator ProductDTO(Product product) => new() 
        { 
            Id = product.Id,
            Name = product.Name,
            BarCode = product.BarCode,
            Price = product.Price,
            CategoryId = product.CategoryId,
            BrandId = product.BrandId
        };
    }
}
