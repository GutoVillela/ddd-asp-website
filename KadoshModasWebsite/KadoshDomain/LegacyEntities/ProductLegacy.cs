using KadoshDomain.Entities;
using KadoshShared.Entities;

namespace KadoshDomain.LegacyEntities
{
    /// <summary>
    /// Entity class for Product in Legacy Database.
    /// </summary>
    public class ProductLegacy : LegacyEntity<Product>
    {
        public ProductLegacy(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? BarCode { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }

        public static implicit operator Product(ProductLegacy legacyProduct)
        {
            Product product = new(
                name: legacyProduct.Name, 
                barCode: legacyProduct.BarCode ?? string.Empty,
                price: legacyProduct.Price,
                categoryId: 0, // At this moment the category and brand is not set from legacy yet
                brandId: 0);

            if (!legacyProduct.IsActive)
                product.Inactivate();

            return product;
        }
    }
}
