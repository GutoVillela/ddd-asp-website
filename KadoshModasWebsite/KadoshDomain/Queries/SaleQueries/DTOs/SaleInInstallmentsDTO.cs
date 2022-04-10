namespace KadoshDomain.Queries.SaleQueries.DTOs
{
    public class SaleInInstallmentsDTO : SaleBaseDTO
    {
        public int NumberOfInstallments { get; set; }
        public IList<SaleInstallmentDTO> Installments { get; set; } = new List<SaleInstallmentDTO>();
    }
}
