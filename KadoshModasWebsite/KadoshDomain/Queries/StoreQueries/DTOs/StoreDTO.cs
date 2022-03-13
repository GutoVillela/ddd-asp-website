using KadoshDomain.Entities;

namespace KadoshDomain.Queries.StoreQueries.DTOs
{
    public class StoreDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        #region Address
        public string? Street { get; set; }

        public string? Number { get; set; }

        public string? Neighborhood { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? ZipCode { get; set; }

        public string? Complement { get; set; }
        #endregion Address

        public static implicit operator StoreDTO(Store store) => new() 
        { 
            Id = store.Id, 
            Name = store.Name ,
            Street = store.Address?.Street,
            Number = store.Address?.Number,
            Neighborhood = store.Address?.Neighborhood,
            City = store.Address?.City,
            State = store.Address?.State,
            ZipCode = store.Address?.ZipCode,
            Complement = store.Address?.Complement
        };
    }
}
