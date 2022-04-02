using System.ComponentModel.DataAnnotations;

namespace KadoshWebsite.Models
{
    public class CashAccountingViewModel
    {
        [Required(ErrorMessage = "A loja é obrigatória")]
        [Display(Name = "Loja")]
        public int StoreId { get; set; }

        [Required(ErrorMessage = "Selecione uma data para calcular o fechamento do caixa")]
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
