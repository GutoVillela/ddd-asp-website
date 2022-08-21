using KadoshDomain.Enums;
using System.ComponentModel.DataAnnotations;

namespace KadoshWebsite.Models
{
    public class CustomerViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "O nome do cliente é obrigatório")]
        [Display(Name = "Cliente")]
        public string? Name { get; set; }

        [Display(Name = "Gênero")]
        public EGender? Gender { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Display(Name = "Tipo")]
        public EDocumentType? DocumentType { get; set; }

        [Display(Name = "Documento")]
        public string? DocumentNumber { get; set; }

        [Display(Name = "Nome de usuário")]
        public string? Username { get; set; }

        #region Address
        [MaxLength(255, ErrorMessage = "O tamanho máximo de caracteres é 255 para o campo Rua")]
        [Display(Name = "Rua")]
        public string? Street { get; set; }

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

        public IList<string>? BoundedCustomers { get; set; } = new List<string>();
        #endregion Address

        public IEnumerable<PhoneViewModel> Phones { get; set; } = new List<PhoneViewModel>();

        public decimal TotalDebt { get; set; } = 0;
    }
}
