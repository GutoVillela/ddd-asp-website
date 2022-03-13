using KadoshDomain.Entities;
using KadoshDomain.Enums;

namespace KadoshDomain.Queries.CustomerPostingQueries.DTOs
{
    public class CustomerPostingDTO
    {
        public int SaleId { get; set; }

        public DateTime PostingDate { get; set; }

        public decimal Value { get; set; }

        public ECustomerPostingType PostingType { get; set; }

        public static implicit operator CustomerPostingDTO(CustomerPosting customerPosting) => new() 
        { 
            SaleId = customerPosting.SaleId,
            PostingDate = customerPosting.PostingDate,
            Value = customerPosting.Value,
            PostingType = customerPosting.Type
        };
    }
}
