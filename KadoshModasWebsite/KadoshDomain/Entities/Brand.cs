using KadoshShared.Entities;
using Flunt.Validations;
using Flunt.Notifications;

namespace KadoshDomain.Entities
{
    public class Brand : Entity
    {
        #region Constructor
        public Brand(string name)
        {
            Name = name;

            ValidateBrand();
        }
        #endregion Constructor

        #region Properties
        public string Name { get; private set; }
        #endregion Properties

        #region Methods
        private void ValidateBrand()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Name, nameof(Name), "Marca inválida!")
            );
        }
        #endregion Methods
    }
}
