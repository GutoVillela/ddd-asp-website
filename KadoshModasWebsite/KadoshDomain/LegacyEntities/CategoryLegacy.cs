using KadoshDomain.Entities;
using KadoshShared.Entities;

namespace KadoshDomain.LegacyEntities
{
    /// <summary>
    /// Entity class for Category in Legacy Database.
    /// </summary>
    public class CategoryLegacy : LegacyEntity<Category>
    {
        public CategoryLegacy(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public static implicit operator Category(CategoryLegacy legacyCategory)
        {
            Category category = new(name: legacyCategory.Name);
            
            if (!legacyCategory.IsActive)
                category.Inactivate();

            return category;
        }
    }
}
