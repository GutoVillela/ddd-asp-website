using Flunt.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KadoshShared.Entities
{
    public abstract class Entity : Notifiable<Notification>
    {
        #region Properties

        public int Id { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime? CreationDate { get; private set; }

        public DateTime? LastUpdateDate { get; private set; }

        #endregion Properties
    }
}
