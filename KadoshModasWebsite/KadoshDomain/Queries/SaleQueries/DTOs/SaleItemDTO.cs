﻿using KadoshDomain.Entities;

namespace KadoshDomain.Queries.SaleQueries.DTOs
{
    public class SaleItemDTO
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public decimal DiscountInPercentage { get; set; } = 0;

        public static implicit operator SaleItemDTO(SaleItem saleItem) => new()
        {
            ProductId = saleItem.ProductId,
            Price = saleItem.Price,
            Amount = saleItem.Amount,
            DiscountInPercentage = saleItem.DiscountInPercentage
        };
    }
}