using KadoshShared.Entities;
using Flunt.Validations;
using Flunt.Notifications;
using KadoshDomain.Enums;
using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.Entities
{
    public class User : Entity
    {

        public User(string username, string password, EUserRole role, int storeId)
        {
            Username = username;
            Password = password;
            Role = role;
            StoreId = storeId;

            ValidateUser();
        }

        public User(string username, string password, EUserRole role, int storeId, Store store) : this(username, password, role, storeId)
        {
            Store = store;
        }

        [Required]
        [MaxLength(20)]
        public string Username { get; private set; }

        [Required]
        public string Password { get; private set; }

        [Required]
        public EUserRole Role { get; private set; }

        [Required]
        public int StoreId { get; private set; }

        public Store? Store { get; private set; }

        public IReadOnlyCollection<Sale> Sales { get; private set; }
        
        
        private void ValidateUser()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Username, nameof(Username), "Nome de usuário inválido!")
                .IsNotNullOrEmpty(Password, nameof(Password), "Senha inválida!")
            );
        }



    }
}
