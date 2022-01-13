using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KadoshDomain.ValueObjects
{
    public class Email : ValueObject
    {
        #region Constructor
        public Email(string emailAddress)
        {
            EmailAddress = emailAddress;

            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsEmail(EmailAddress, nameof(EmailAddress), "E-mail inválido")
            );
        }
        #endregion Constructor

        #region Properties
        public string EmailAddress { get; set; }
        #endregion Properties
    }
}
