using KadoshDomain.Entities;
using KadoshDomain.Enums;

namespace KadoshDomain.Queries.UserQueries.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public EUserRole Role { get; set; }

        public int StoreId { get; set; }

        public static implicit operator UserDTO(User user) => new()
        {
            Id = user.Id,
            Name = user.Name,
            UserName = user.Username,
            Role = user.Role,
            StoreId = user.StoreId
        };
    }
}
