﻿using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshShared.Entities;
using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.Entities
{
    public class SaleItem : Entity, ICloneable
    {
        #region Constructors
        public SaleItem(int saleId, int productId, int amount, decimal price, decimal discountInPercentage, ESaleItemSituation situation)
        {
            SaleId= saleId;
            ProductId= productId;
            Amount = amount;
            Price = price;
            DiscountInPercentage = discountInPercentage;
            Situation = situation;

            ValidateSaleItem();
        }

        public SaleItem(Product product, int amount, decimal price, decimal discountInPercentage, ESaleItemSituation situation)
        {
            Product = product;
            Amount = amount;
            Price = price;
            DiscountInPercentage = discountInPercentage;
            Situation = situation;

            ValidateSaleItem();
        }

        public SaleItem(int saleId, int productId, int amount, decimal price, decimal discountInPercentage, ESaleItemSituation situation, Sale? sale) 
            : this(saleId, productId, amount, price, discountInPercentage, situation)
        {
            Sale = sale;
        }

        public SaleItem(int saleId, int productId, int amount, decimal price, decimal discountInPercentage, ESaleItemSituation situation, Sale? sale, Product? product)
            : this(saleId, productId, amount, price, discountInPercentage, situation, sale)
        {
            Product = product;
        }
        #endregion Constructors

        [Required]
        public int SaleId { get; private set; }

        public Sale? Sale { get; private set; }  

        [Required]
        public int ProductId { get; private set; }

        public Product? Product { get; private set; }

        [Required]
        public int Amount { get; private set; }

        [Required]
        public decimal Price { get; private set; }

        [Required]
        [Range(0, 100)]
        public decimal DiscountInPercentage { get; private set; }

        [Required]
        public ESaleItemSituation Situation { get; private set; }

        public void UpdateSituation(ESaleItemSituation itemSituation)
        {
            Situation = itemSituation;
        }

        public void DecreaseAmount(int amountToDecrease)
        {
            if (amountToDecrease < 0 || amountToDecrease > Amount)
                throw new ArgumentException("Invalid amount to decrease. AmountToDecrease must be lower or equals item amount");

            Amount -= amountToDecrease;
        }

        public void SetAmount(int amount)
        {
            if (amount < 1)
                throw new ArgumentException("Invalid amount for sale item");

            Amount = amount;
        }

        public object Clone()
        {
            return new SaleItem(
                saleId: SaleId,
                productId: ProductId,
                amount: Amount,
                price: Price,
                discountInPercentage: DiscountInPercentage,
                situation: Situation,
                sale: Sale,
                product: Product);
        }

        public decimal CalculateSaleItemTotal()
        {
            return (Price * (DiscountInPercentage/100)) * Amount;
        }

        private void ValidateSaleItem()
        {
            AddNotifications(new Contract<Notification>()
               .Requires()
               .IsGreaterThan(Amount, 0, nameof(Amount), "A quantidade de itens da venda não pode ser menor do que 1!")
               .IsGreaterOrEqualsThan(Price, 0, nameof(Price), "O preço do item da venda não pode ser negativo!")
               .IsBetween(DiscountInPercentage, 0, 100, nameof(DiscountInPercentage), "O desconto do item da venda deve estar entre 0 e 100%!")
           );
        }

        
    }
}
