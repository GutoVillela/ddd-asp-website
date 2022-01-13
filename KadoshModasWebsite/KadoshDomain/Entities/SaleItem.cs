using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshShared.Entities;

namespace KadoshDomain.Entities
{
    public class SaleItem : Entity
    {
        #region Constructor
        public SaleItem(Product product, int amount, decimal price, decimal discountInPercentage, ESaleItemSituation situation)
        {
            Product = product;
            Amount = amount;
            Price = price;
            DiscountInPercentage = discountInPercentage;
            Situation = situation;

            ValidateSaleItem();
        }
        #endregion Constructor

        #region Properties
        public Product Product { get; private set; }
        public int Amount { get; private set; }
        public decimal Price { get; private set; }
        public decimal DiscountInPercentage { get; private set; }
        public ESaleItemSituation Situation { get; private set; }
        #endregion Properties

        #region Methods
        private void ValidateSaleItem()
        {
            AddNotifications(new Contract<Notification>()
               .Requires()
               .IsGreaterThan(Amount, 0, nameof(Amount), "A quantidade de itens da venda não pode ser menor do que 1!")
               .IsGreaterOrEqualsThan(Price, 0, nameof(Price), "O preço do item da venda não pode ser negativo!")
               .IsBetween(DiscountInPercentage, 0, 100, nameof(DiscountInPercentage), "O desconto do item da venda deve estar entre 0 e 100%!")
           );
        }
        #endregion Methods
    }
}
