using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Entities;

namespace KadoshDomain.Entities
{
    public class Stock : Entity
    {
        #region Constructor
        public Stock(Product product, int amountInStock, int minimumAmountBeforeLowStock)
        {
            Product = product;
            AmountInStock = amountInStock;
            MinimumAmountBeforeLowStock = minimumAmountBeforeLowStock;
            
            ValidateStock();
        }
        #endregion Constructor

        #region Properties
        public Product Product { get; private set; }
        public int AmountInStock { get; private set; }
        public int MinimumAmountBeforeLowStock { get; private set; }
        #endregion Properties

        #region Methods
        private void ValidateStock()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsGreaterOrEqualsThan(AmountInStock, 0, nameof(AmountInStock), "Não é possível um número negativo de produtos em estoque!")
                .IsGreaterOrEqualsThan(MinimumAmountBeforeLowStock, 0, nameof(MinimumAmountBeforeLowStock), "Não é possível um número negativo para mínimo aceitável em estoque!")
            );
        }
        #endregion Methods
    }
}
