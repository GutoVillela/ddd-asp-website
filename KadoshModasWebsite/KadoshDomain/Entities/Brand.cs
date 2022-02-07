using KadoshShared.Entities;
using Flunt.Validations;
using Flunt.Notifications;
using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.Entities
{
    public class Brand : Entity
    {
        public Brand(string name)
        {
            Name = name;

            ValidateBrand();
        }

        public Brand(string name, IReadOnlyCollection<Product> products) : this(name)
        {
            Products = products;
        }

        [Required]
        [MaxLength(100)]
        public string Name { get; private set; }

        public IReadOnlyCollection<Product> Products { get; private set; } = new List<Product>();

        public void UpdateBrandInfo(string name)
        {
            Name = name;
            LastUpdateDate = DateTime.UtcNow;
            ValidateBrand();
        }

        private void ValidateBrand()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Name, nameof(Name), "Marca inválida!")
            );
        }
    }
}
