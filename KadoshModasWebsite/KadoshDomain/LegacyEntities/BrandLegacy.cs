using KadoshDomain.Entities;
using KadoshShared.Entities;

namespace KadoshDomain.LegacyEntities
{
    /// <summary>
    /// Entity class for Brand in Legacy Database.
    /// </summary>
    public class BrandLegacy : LegacyEntity<Brand>
    {
        public BrandLegacy(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public static implicit operator Brand(BrandLegacy legacyBrand)
        {
            Brand brand = new(name: legacyBrand.Name);
            if (!legacyBrand.IsActive)
                brand.Inactivate();
            return brand;
        }
    }
}
