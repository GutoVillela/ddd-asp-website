﻿using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.ValueObjects;

namespace KadoshDomain.Queries.CustomerQueries.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public EGender? Gender { get; set; }

        public string? Email { get; set; }

        public EDocumentType? DocumentType { get; set; }

        public string? DocumentNumber { get; set; }

        public string? Username { get; set; }

        public ICollection<Phone> Phones { get; set; } = new HashSet<Phone>();

        public ICollection<CustomerDTO>? BoundedCustomers { get; set; } = new HashSet<CustomerDTO>();

        #region Address
        public string? Street { get; set; }

        public string? Number { get; set; }

        public string? Neighborhood { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? ZipCode { get; set; }

        public string? Complement { get; set; }
        #endregion Address

        public static implicit operator CustomerDTO(Customer customer)
        {
            CustomerDTO customerDTO = new()
            {
                Id = customer.Id,
                Name = customer.Name,
                Gender = customer.Gender,
                Email = customer.Email?.EmailAddress,
                DocumentType = customer.Document?.Type,
                DocumentNumber = customer.Document?.Number,
                Phones = customer.Phones?.ToHashSet() ?? new HashSet<Phone>(),
                Street = customer.Address?.Street,
                Number = customer.Address?.Number,
                Neighborhood = customer.Address?.Neighborhood,
                City = customer.Address?.City,
                State = customer.Address?.State,
                ZipCode = customer.Address?.ZipCode,
                Complement = customer.Address?.Complement,
                Username = customer.Username
            };

            if(customer.BoundedCustomers is not null)
                customerDTO.BoundedCustomers = GetCustomerDTOsFromCustomers(customer.BoundedCustomers.ToHashSet());

            return customerDTO;
        }

        private static ICollection<CustomerDTO> GetCustomerDTOsFromCustomers(ICollection<Customer> customers)
        {
            ICollection<CustomerDTO> customerDTOs = new HashSet<CustomerDTO>();
            foreach(var customer in customers)
            {
                customerDTOs.Add(customer);
            }
            return customerDTOs;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;

            if(obj is not CustomerDTO)
                return false;

            CustomerDTO dto = obj as CustomerDTO;
            return dto.Id.Equals(Id);
        }
    }
}
