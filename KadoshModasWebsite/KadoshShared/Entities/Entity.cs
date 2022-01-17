using Flunt.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KadoshShared.Entities
{
    public abstract class Entity : Notifiable<Notification>
    {
        protected Entity() { }

        protected Entity(int id, bool isActive, DateTime creationDate, DateTime? lastUpdateDate)
        {
            Id = id;
            IsActive = isActive;
            CreationDate = creationDate;
            LastUpdateDate = lastUpdateDate;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        public bool IsActive { get; private set; } = true;

        public DateTime CreationDate { get; private set; } = DateTime.UtcNow;

        public DateTime? LastUpdateDate { get; private set; }
    }
}
