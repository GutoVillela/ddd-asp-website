using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
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
        public int Id { get; protected set; }

        public bool IsActive { get; protected set; } = true;

        public DateTime CreationDate { get; protected set; } = DateTime.UtcNow;

        public DateTime? LastUpdateDate { get; protected set; }

        public void SetLastUpdateDate(DateTime lastUpdateDate)
        {
            LastUpdateDate = lastUpdateDate;
            ValidateLastUpdateDate();
        }

        private void ValidateLastUpdateDate()
        {
            if(LastUpdateDate.HasValue)
                AddNotifications(new Contract<Notification>()
                    .Requires()
                    .IsGreaterThan(LastUpdateDate.Value, CreationDate, nameof(LastUpdateDate), EntityValidationErrors.LAST_UPDATE_DATETIME_BEFORE_CREATION_TIME_ERROR)
                );
        }
    }
}
