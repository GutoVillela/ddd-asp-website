using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Entities;

namespace KadoshDomain.Entities
{
    public class Product : Entity
    {
        #region Constructor
        public Product(string name, string barCode, decimal price, Category category, Brand brand)
        {
            Name = name;
            BarCode = barCode;
            Price = price;
            Category = category;
            Brand = brand;

            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Name, nameof(Name), "Nome do produto inválido!")
                .IsGreaterThan(Price, 0, nameof(Price), "Preço do produto inválido!")
            );
        }
        #endregion Constructor

        #region Properties
        public string Name { get; private set; }
        public string BarCode { get; private set; }
        public decimal Price { get; private set; }
        public Category Category { get; private set; }
        public Brand Brand { get; private set; }
        #endregion Properties
    }
}
