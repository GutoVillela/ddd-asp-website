using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Entities;

namespace KadoshDomain.Entities
{
    public class Category : Entity
    {
        #region Constructor
        public Category(string name)
        {
            Name = name;

            ValidateCategory();
        }
        #endregion Constructor

        #region Properties
        public string Name { get; private set; }
        #endregion Properties

        #region Methods
        private void ValidateCategory()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Name, nameof(Name), "Categoria inválida!")
            );
        }
        #endregion Methods
    }
}
