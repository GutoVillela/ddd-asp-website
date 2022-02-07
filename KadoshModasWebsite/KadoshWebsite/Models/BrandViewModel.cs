using System.ComponentModel.DataAnnotations;

namespace KadoshWebsite.Models
{
    public class BrandViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "O nome da marca é obrigatório")]
        [Display(Name = "Marca")]
        public string? Name { get; set; }
    }
}
