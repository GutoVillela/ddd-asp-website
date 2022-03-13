using System.ComponentModel.DataAnnotations;

namespace KadoshWebsite.Models
{
    public class ProductViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório")]
        [Display(Name = "Produto")]
        public string? Name { get; set; }

        [Display(Name = "Código de Barras")]
        public string? BarCode { get; set; }

        [Required(ErrorMessage = "O preço do produto é obrigatório")]
        [Display(Name = "Preço")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "A categoria do produto é obrigatório")]
        [Display(Name = "Categoria")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "A marca do produto é obrigatório")]
        [Display(Name = "Marca")]
        public int BrandId { get; set; }
    }
}
