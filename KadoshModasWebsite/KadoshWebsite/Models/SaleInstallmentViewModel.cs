using KadoshDomain.Enums;

namespace KadoshWebsite.Models
{
    public class SaleInstallmentViewModel
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime MaturityDate { get; set; }
        public decimal Value { get; set; }
        public EInstallmentSituation Situation { get; set; }
    }
}
