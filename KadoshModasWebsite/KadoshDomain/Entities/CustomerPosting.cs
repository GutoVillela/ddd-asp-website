using KadoshDomain.Enums;
using KadoshDomain.ExtensionMethods;
using KadoshShared.Entities;
using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.Entities
{
    public class CustomerPosting : Entity
    {
        public CustomerPosting(ECustomerPostingType type, decimal value, int saleId, DateTime postingDate)
        {
            Type = type;
            Value = value;
            SaleId = saleId;
            PostingDate = postingDate;
        }

        public CustomerPosting(ECustomerPostingType type, decimal value, Sale sale, DateTime postingDate)
        {
            Type = type;
            Value = value;
            Sale = sale;
            PostingDate = postingDate;
        }

        [Required]
        public ECustomerPostingType Type { get; private set; }

        [Required]
        public decimal Value { get; private set; }

        [Required]
        public int SaleId { get; private set; }

        public Sale? Sale { get; private set; }

        [Required]
        public DateTime PostingDate { get; private set; }

        public bool IsCreditType()
        {
            return Type.IsCreditType();
        }
    }
}
