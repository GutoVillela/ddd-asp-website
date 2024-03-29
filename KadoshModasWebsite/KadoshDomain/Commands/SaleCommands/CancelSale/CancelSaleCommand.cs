﻿using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands.SaleCommands.CancelSale
{
    public class CancelSaleCommand : Notifiable<Notification>, ICommand
    {
        public int? SaleId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(SaleId, nameof(SaleId), SaleValidationsErrors.NULL_SALE_ID)
            );
        }
    }
}
