using KadoshDomain.Enums;
using System.ComponentModel.DataAnnotations;

namespace KadoshWebsite.Models
{
    public class CustomerPostingViewModel
    {
        [Display(Name = "Venda")]
        public int SaleId { get; set; }

        [Display(Name = "Data do lançamento")]
        public DateTime PostingDate { get; set; }

        [Display(Name = "Valor do lançamento")]
        public decimal Value { get; set; }

        [Display(Name = "Tipo de Lançamento")]
        public ECustomerPostingType PostingType { get; set; }
    }
}
