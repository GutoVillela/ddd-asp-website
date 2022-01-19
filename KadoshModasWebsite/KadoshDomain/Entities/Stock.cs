using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Entities;
using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.Entities
{
    public class Stock : Entity
    {
        public Stock(int productId, int amountInStock, int minimumAmountBeforeLowStock, int storeId)
        {
            ProductId = productId;
            AmountInStock = amountInStock;
            MinimumAmountBeforeLowStock = minimumAmountBeforeLowStock;
            StoreId = storeId;
            
            ValidateStock();
        }

        public Stock(int productId, int amountInStock, int minimumAmountBeforeLowStock, int storeId, Product? product) : this(productId, amountInStock, minimumAmountBeforeLowStock, storeId)
        {
            Product = product;
        }

        public Stock(int productId, int amountInStock, int minimumAmountBeforeLowStock, int storeId, Product? product, Store? store) : this(productId, amountInStock, minimumAmountBeforeLowStock, storeId, product)
        {
            Store = store;
        }

        [Required]
        public int ProductId { get; private set; }

        public Product? Product { get; private set; }

        [Required]
        public int AmountInStock { get; private set; }

        [Required]
        public int MinimumAmountBeforeLowStock { get; private set; }

        [Required]
        public int StoreId { get; private set; }

        public Store? Store { get; private set; }

        private void ValidateStock()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsGreaterOrEqualsThan(AmountInStock, 0, nameof(AmountInStock), "Não é possível um número negativo de produtos em estoque!")
                .IsGreaterOrEqualsThan(MinimumAmountBeforeLowStock, 0, nameof(MinimumAmountBeforeLowStock), "Não é possível um número negativo para mínimo aceitável em estoque!")
            );
        }
    }
}
