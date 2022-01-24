using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.ValueObjects;
using KadoshShared.Entities;
using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.Entities
{
    public class Store : Entity
    {
        private Store(string name)
        {
            Name = name;

            ValidateStore();
        }

        public Store(string name, Address address) : this(name)
        {
            Address = address;
        }

        public Store(int id, string name, Address address) : this(name, address)
        {
            Id = id;
        }

        [Required]
        [MaxLength(255)]
        public string Name { get; private set; }

        [Required]
        public Address Address { get; private set; }

        public IReadOnlyCollection<User> Users { get; private set; }

        public IReadOnlyCollection<Stock> Stocks { get; private set; }

        private void ValidateStore()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Name, nameof(Name), "Nome da loja inválido!")
            );
        }
    }
}