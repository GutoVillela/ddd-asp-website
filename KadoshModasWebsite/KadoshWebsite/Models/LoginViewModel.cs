using KadoshDomain.Enums;
using System.ComponentModel.DataAnnotations;

namespace KadoshWebsite.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório")]
        [MaxLength(20, ErrorMessage = "O tamanho máximo para o nome de usuário é de 20 caracteres")]
        [Display(Name = "Nome de usuário")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        [MinLength(4, ErrorMessage = "A senha precisa ter pelo menos 4 caracteres")]
        public string Password { get; set; }

    }
}