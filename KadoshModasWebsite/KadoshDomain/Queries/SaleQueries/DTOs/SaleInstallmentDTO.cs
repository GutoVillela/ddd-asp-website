using KadoshDomain.Entities;
using KadoshDomain.Enums;

namespace KadoshDomain.Queries.SaleQueries.DTOs
{
    public class SaleInstallmentDTO
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime MaturityDate { get; set; }
        public decimal Value { get; set; }
        public EInstallmentSituation Situation { get; set; }

        public static implicit operator SaleInstallmentDTO(Installment installment) => new()
        {
            Id = installment.Id,
            Number = installment.Number,
            MaturityDate = installment.MaturityDate,
            Value = installment.Value,
            Situation = installment.Situation
        };
    }
}
