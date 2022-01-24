using System.ComponentModel.DataAnnotations;

namespace KadoshWebsite.Models
{
    public class StoreViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "O nome da loja é obrigatório")]
        [Display(Name ="Loja")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "A rua da loja é obrigatório")]
        [MaxLength(255, ErrorMessage = "O tamanho máximo de caracteres é 255 para o campo Rua")]
        [Display(Name = "Rua")]
        public string? Street { get; set; }

        [Required(ErrorMessage = "O número do endereço da loja é obrigatório")]
        [MaxLength(20, ErrorMessage = "O tamanho máximo de caracteres é 20 para o campo Número")]
        [Display(Name = "Número")]
        public string? Number { get; set; }

        [MaxLength(255, ErrorMessage = "O tamanho máximo de caracteres é 255 para o campo Bairro")]
        [Display(Name = "Bairro")]
        public string? Neighborhood { get; set; }

        [MaxLength(50, ErrorMessage = "O tamanho máximo de caracteres é 50 para o campo Cidade")]
        [Display(Name = "Cidade")]
        public string? City { get; set; }

        [MaxLength(20, ErrorMessage = "O tamanho máximo de caracteres é 20 para o campo Estado")]
        [Display(Name = "Estado")]
        public string? State { get; set; }

        [MaxLength(10, ErrorMessage = "O tamanho máximo de caracteres é 10 para o campo CEP")]
        [Display(Name = "CEP")]
        public string? ZipCode { get; set; }

        [MaxLength(255, ErrorMessage = "O tamanho máximo de caracteres é 255 para o campo Complemento")]
        [Display(Name = "Complemento")]
        public string? Complement { get; set; }
    }
}
