﻿using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Commands.SaleCommands.Base;
using KadoshDomain.Entities;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands.SaleCommands.CreateSaleInInstallments
{
    public class CreateSaleInInstallmentsCommand : CreateSaleCommandBase
    {
        public decimal InterestOnTheTotalSaleInPercentage { get; set; } = 0;

        public IList<Installment> Installments { get; set; } = new List<Installment>();// TODO check if it's possible not to use Entity here

        public override void Validate()
        {
            base.Validate();

            if (Installments is null)
                AddNotification(nameof(Installments), SaleValidationsErrors.EMPTY_SALE_INSTALLMENTS);
            else
                AddNotifications(new Contract<Notification>()
                    .Requires()
                    .IsGreaterOrEqualsThan(Installments.Count, 2, nameof(Installments), SaleValidationsErrors.LESS_THAN_TWO_INSTALLMENTS_ERROR)
                );

            if(InterestOnTheTotalSaleInPercentage < 0)
                AddNotification(nameof(InterestOnTheTotalSaleInPercentage), SaleValidationsErrors.NEGATIVE_INTEREST_ON_TOTAL_SALE_ERROR);
        }
    }
}
