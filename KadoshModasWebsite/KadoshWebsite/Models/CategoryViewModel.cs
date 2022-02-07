using System.ComponentModel.DataAnnotations;

namespace KadoshWebsite.Models
{
    public class CategoryViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "O nome da categoria é obrigatório")]
        [Display(Name = "Categoria")]
        public string? Name { get; set; }
    }
}
