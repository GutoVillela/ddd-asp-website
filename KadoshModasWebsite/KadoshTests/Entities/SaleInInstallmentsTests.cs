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
        [TestMethod]
        public void ShouldReturnErrorWhenSaleInInstallmentsHasNoInstallments()
        {
            Customer customer = new("Bryam Adams");
            EFormOfPayment formOfPayment = EFormOfPayment.Cash;
            decimal discountInPercentage = 10;
            decimal downPayment = 100;
            DateTime saleDate = DateTime.Now;
            List<SaleItem> saleItems = CreateSaleItens(10).ToList();

            var saleInInstallment = new SaleInInstallments(
                customer: customer,
                formOfPayment: formOfPayment,
                discountInPercentage: discountInPercentage,
                downPayment: downPayment,
                saleDate: saleDate,
                saleItems: saleItems,
                situation: ESaleSituation.Open,
                installments: new List<Installment>(),// There's no installments for this sale
                interestOnTheTotalSaleInPercentage: 2
                );

            Assert.IsFalse(saleInInstallment.IsValid);
        }

        [TestMethod]
        public void ShouldReturnSuccessWhenSaleInInstallmentsHasAtLeastOneInstallment()
        {
            Customer customer = new("Bryam Adams");
            EFormOfPayment formOfPayment = EFormOfPayment.Cash;
            decimal discountInPercentage = 10;
            decimal downPayment = 100;
            DateTime saleDate = DateTime.Now;
            List<SaleItem> saleItems = CreateSaleItens(10).ToList();

            var saleInInstallment = new SaleInInstallments(
                customer: customer,
                formOfPayment: formOfPayment,
                discountInPercentage: discountInPercentage,
                downPayment: downPayment,
                saleDate: saleDate,
                saleItems: saleItems,
                situation: ESaleSituation.Open,
                installments: CreateInstallments(10).ToList(),// There are installments for this sale
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

                Product product = new($"Product {i}", barCode, price, category, brand);

                saleItems.Add(new SaleItem(product, amount, price, discount, ESaleItemSituation.AcquiredOnPurchase));
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

                installments.Add(new Installment(number, value, maturityDate, situation));
            }

            return installments;
        }
    }
}