using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.ValueObjects;
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

        private readonly User _seller;

        private readonly Store _store;

        private readonly Address _address;

        private readonly ICollection<SaleItem> _saleItems;

        private readonly ICollection<Installment> _installments;

        public SaleInInstallmentsTests()
        {
            _customer = new("Bryam Adams");
            _saleItems = CreateSaleItens(10);
            _installments = CreateInstallments(10);
            _seller = new("Vendedor", "Vendedor", "senha", new byte[1], 0, EUserRole.Seller, 1);
            _address = new(
                street: "Street",
                number: "10",
                neighborhood: "Neighborhood",
                city: "City",
                state: "State",
                zipCode: "00001-000",
                complement: "House 2"
                );
            _store = new(name: "Loja", address: _address);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenSaleInInstallmentsHasNoInstallments()
        {
            EFormOfPayment formOfPayment = EFormOfPayment.Cash;
            decimal discountInPercentage = 10;
            decimal downPayment = 100;
            DateTime saleDate = DateTime.Now;

            var saleInInstallment = new SaleInInstallments(
                customer: _customer,
                formOfPayment: formOfPayment,
                discountInPercentage: discountInPercentage,
                downPayment: downPayment,
                saleDate: saleDate,
                situation: ESaleSituation.Open,
                seller: _seller,
                store: _store,
                saleItems: _saleItems as IReadOnlyCollection<SaleItem>,
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
                customer: _customer,
                formOfPayment: formOfPayment,
                discountInPercentage: discountInPercentage,
                downPayment: downPayment,
                saleDate: saleDate,
                situation: ESaleSituation.Open,
                seller: _seller,
                store: _store,
                saleItems: _saleItems as IReadOnlyCollection<SaleItem>,
                installments: _installments as IReadOnlyCollection<Installment>,// There's at least one installment
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