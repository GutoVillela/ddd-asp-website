using KadoshDomain.Enums;
using KadoshWebsite.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KadoshWebsite.Models
{
    public class SaleViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "O cliente é obrigatório")]
        [Display(Name = "Cliente")]
        public int CustomerId { get; set; }

        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "O vendedor é obrigatório")]
        [Display(Name = "Vendedor")]
        public int SellerId { get; set; }

        [Required(ErrorMessage = "A loja é obrigatória")]
        [Display(Name = "Loja")]
        public int StoreId { get; set; }

        public string StoreName { get; set; } = string.Empty;

        public IEnumerable<SaleItemViewModel> SaleItems { get; set; } = new List<SaleItemViewModel>();

        [Required(ErrorMessage = "O tipo de pagamento é obrigatório")]
        [Display(Name = "Tipo de pagamento")]
        public ESalePaymentType PaymentType { get; set; }

        [Display(Name = "Parcelas")]
        public int NumberOfInstallments { get;set; }

        public IEnumerable<SelectListItem> Installments { get; } = new List<SelectListItem> ()
        {
            new SelectListItem { Text = "2x", Value = "2" },
            new SelectListItem { Text = "3x", Value = "3" },
            new SelectListItem { Text = "4x", Value = "4" },
            new SelectListItem { Text = "5x", Value = "5" },
            new SelectListItem { Text = "6x", Value = "6" },
            new SelectListItem { Text = "7x", Value = "7" },
            new SelectListItem { Text = "8x", Value = "8" },
            new SelectListItem { Text = "9x", Value = "9" },
            new SelectListItem { Text = "10x", Value = "10" },
            new SelectListItem { Text = "11x", Value = "11" },
            new SelectListItem { Text = "12x", Value = "12" },
        };

        [Display(Name = "Entrada")]
        public decimal? DownPayment { get; set; } = 0;

        public string? SaleTotalFormatted { get; set; }

        public DateTime? SaleDate { get; set; }

        public ESaleSituation Status { get; set; }

        public decimal TotalPaid { get; set; } = 0;

        public decimal TotalToPay { get; set; } = 0;
    }
}
