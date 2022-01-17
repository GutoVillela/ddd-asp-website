using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Entities;
using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.Entities
{
    public class Category : Entity
    {
        public Category(string name)
        {
            Name = name;

            ValidateCategory();
        }

        public Category(string name, IReadOnlyCollection<Product> products) : this(name)
        {
            Products = products;
        }

        [Required]
        [MaxLength(100)]
        public string Name { get; private set; }

        public IReadOnlyCollection<Product> Products { get; private set; } = new List<Product>();

        private void ValidateCategory()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Name, nameof(Name), "Categoria inválida!")
            );
        }
    }
}
