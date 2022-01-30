using KadoshShared.Entities;
using Flunt.Validations;
using Flunt.Notifications;
using KadoshDomain.Enums;
using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.Entities
{
    public class User : Entity
    {

        public User(string name, string username, string passwordHash, EUserRole role, int storeId)
        {
            Name = name;
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
            StoreId = storeId;

            ValidateUser();
        }

        [Required]
        [MaxLength(100)]
        public string Name { get; private set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; private set; }

        [Required]
        public string PasswordHash { get; private set; }

        [Required]
        public EUserRole Role { get; private set; }

        [Required]
        public int StoreId { get; private set; }

        public Store? Store { get; private set; }

        public IReadOnlyCollection<Sale>? Sales { get; private set; }

        public void UpdateUserInfo(string name, string username, string passwordHash, EUserRole role, int storeId)
        {
            Name = name;
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
            StoreId = storeId;

            ValidateUser();
        }
        private void ValidateUser()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Name, nameof(Name), "Nome inválido")
                .IsNotNullOrEmpty(Username, nameof(Username), "Nome de usuário inválido")
                .IsNotNullOrEmpty(PasswordHash, nameof(PasswordHash), "Senha inválida")
            );
        }

    }
}
