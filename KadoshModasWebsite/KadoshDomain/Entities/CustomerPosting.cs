using KadoshDomain.Enums;
using KadoshShared.Entities;
using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.Entities
{
    public class CustomerPosting : Entity
    {

        public CustomerPosting(int customerId, ECustomerPostingType type, decimal value, int saleId, DateTime postingDate)
        {
            CustomerId = customerId;
            Type = type;
            Value = value;
            SaleId = saleId;
            PostingDate = postingDate;
        }

        public CustomerPosting(int customerId, Customer customer, ECustomerPostingType type, decimal value, int saleId, Sale sale, DateTime postingDate)
        {
            Customer = customer;
            Type = type;
            Value = value;
            Sale = sale;
            PostingDate = postingDate;
        }

        [Required]
        public int CustomerId { get; private set; }

        public Customer? Customer { get; private set; }

        [Required]
        public ECustomerPostingType Type { get; private set; }

        [Required]
        public decimal Value { get; private set; }

        [Required]
        public int SaleId { get; private set; }

        public Sale? Sale { get; private set; }

        [Required]
        public DateTime PostingDate { get; set; }
    }
}
