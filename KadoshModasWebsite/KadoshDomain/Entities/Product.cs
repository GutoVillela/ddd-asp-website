﻿using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Entities;
using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.Entities
{
    public class Product : Entity
    {
        public Product(string name, string barCode, decimal price, int categoryId, int brandId)
        {
            Name = name;
            BarCode = barCode;
            Price = price;
            CategoryId = categoryId;
            BrandId = brandId;

            ValidateProduct();
        }

        public Product(string name, string barCode, decimal price, int categoryId, int brandId, int? stockId, Category category, Brand brand, Stock stock) : this(name, barCode, price, categoryId, brandId)
        {
            Category = category;
            Brand = brand;
        }

        [Required]
        [MaxLength(255)]
        public string Name { get; private set; }

        public string? BarCode { get; private set; }

        [Required]
        public decimal Price { get; private set; }

        [Required]
        public int CategoryId { get; private set; }

        public Category? Category { get; private set; }

        [Required]
        public int BrandId { get; private set; }

        public Brand? Brand { get; private set; }

        public IReadOnlyCollection<SaleItem> SaleItems { get; private set; }

        private void ValidateProduct()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Name, nameof(Name), "Nome do produto inválido!")
                .IsGreaterThan(Price, 0, nameof(Price), "Preço do produto inválido!")
            );
        }
    }
}
