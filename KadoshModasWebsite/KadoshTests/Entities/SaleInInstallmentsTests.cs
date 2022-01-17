using KadoshDomain.Entities;
using KadoshDomain.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KadoshTests.Entities
{
    [TestClass]
    public class SaleInInstallmentsTests
    {
        private readonly Customer _customer;

        private readonly ICollection<SaleItem> _saleItems;

        private readonly ICollection<Installment> _installments;

        public SaleInInstallmentsTests()
        {
            _customer = new("Bryam Adams");
            _saleItems = CreateSaleItens(10);
            _installments = CreateInstallments(10);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenSaleInInstallmentsHasNoInstallments()
        {
            EFormOfPayment formOfPayment = EFormOfPayment.Cash;
            decimal discountInPercentage = 10;
            decimal downPayment = 100;
            DateTime saleDate = DateTime.Now;

            var saleInInstallment = new SaleInInstallments(
                customerId: _customer.Id,
                formOfPayment: formOfPayment,
                discountInPercentage: discountInPercentage,
                downPayment: downPayment,
                saleDate: saleDate,
                saleItems: _saleItems.ToList(),
                situation: ESaleSituation.Open,
                installments: new List<Installment>(),// There's no installments for this sale
                interestOnTheTotalSaleInPercentage: 2
                );

            Assert.IsFalse(saleInInstallment.IsValid);
        }

        [TestMethod]
        public void ShouldReturnSuccessWhenSaleInInstallmentsHasAtLeastOneInstallment()
        {
            EFormOfPayment formOfPayment = EFormOfPayment.Cash;
            decimal discountInPercentage = 10;
            decimal downPayment = 100;
            DateTime saleDate = DateTime.Now;

            var saleInInstallment = new SaleInInstallments(
                customerId: _customer.Id,
                formOfPayment: formOfPayment,
                discountInPercentage: discountInPercentage,
                downPayment: downPayment,
                saleDate: saleDate,
                saleItems: _saleItems.ToList(),
                situation: ESaleSituation.Open,
                installments: _installments.ToList(),// There are installments for this sale
                interestOnTheTotalSaleInPercentage: 2
                );

            Assert.IsTrue(saleInInstallment.IsValid);
        }

        private ICollection<SaleItem> CreateSaleItens(int amountOfSaleItens)
        {
            List<SaleItem> saleItems = new();
            for (int i = 0; i < amountOfSaleItens; i++)
            {
                string barCode = string.Empty;
                int amount = 1;
                Category category = new($"Category {i}");
                decimal price = 5.0m * i;
                decimal discount = 0;
                Brand brand = new($"Brand {i}");

                Product product = new($"Product {i}", barCode, price, category.Id, brand.Id);

                saleItems.Add(new SaleItem(0, product.Id, amount, price, discount, ESaleItemSituation.AcquiredOnPurchase));
            }

            return saleItems;
        }

        private ICollection<Installment> CreateInstallments(int amountOfInstallments)
        {
            List<Installment> installments = new();
            for (int i = 0; i < amountOfInstallments; i++)
            {
                int number = i;
                int value = i * 10;
                DateTime maturityDate = DateTime.Now;
                EInstallmentSituation situation = EInstallmentSituation.Open;

                installments.Add(new Installment(number, value, maturityDate, situation, 0, null));
            }

            return installments;
        }
    }
}