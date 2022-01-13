using KadoshShared.Entities;
using Flunt.Validations;
using Flunt.Notifications;
using KadoshDomain.Enums;

namespace KadoshDomain.Entities
{
    public class User : Entity
    {
        #region Constructor

        public User(string username, string password, EUserRole role)
        {
            Username = username;
            Password = password;
            Role = role;

            ValidateUser();
        }

        #endregion Constructor

        #region Properties
        public string Username { get; private set; }
        public string Password { get; private set; }
        public EUserRole Role { get; private set; }
        #endregion Properties
        
        
        #region Methods
        private void ValidateUser()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Username, nameof(Username), "Nome de usuário inválido!")
                .IsNotNullOrEmpty(Password, nameof(Password), "Senha inválida!")
            );
        }
        #endregion Methods



    }
}
