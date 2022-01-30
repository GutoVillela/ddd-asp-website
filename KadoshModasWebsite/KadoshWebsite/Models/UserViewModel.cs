using KadoshDomain.Enums;
using System.ComponentModel.DataAnnotations;

namespace KadoshWebsite.Models
{
    public class UserViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [MaxLength(100, ErrorMessage = "O tamanho máximo para o nome é de 100 caracteres")]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O nome de usuário é obrigatório")]
        [MaxLength(20, ErrorMessage = "O tamanho máximo para o nome de usuário é de 20 caracteres")]
        [Display(Name = "Nome de usuário")]
        public string UserName { get; set; }

        public string? UserNameBeforeEdit { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        [MinLength(4, ErrorMessage = "A senha precisa ter pelo menos 4 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Você precisa confirmar a senha")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "As senhas não são iguais")]
        [Display(Name = "Confirme a senha")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "O cargo é obrigatório")]
        [Display(Name = "Cargo")]
        public EUserRole Role { get; set; }

        [Required(ErrorMessage = "A loja do usuário é obrigatório")]
        [Display(Name = "Loja")]
        public int StoreId { get; set; }

    }
}