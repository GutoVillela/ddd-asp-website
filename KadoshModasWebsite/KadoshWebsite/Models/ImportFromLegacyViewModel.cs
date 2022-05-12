using System.ComponentModel.DataAnnotations;

namespace KadoshWebsite.Models
{
    public class ImportFromLegacyViewModel
    {
        [Required(ErrorMessage = "O servidor é obrigatório")]
        [Display(Name = "Servidor")]
        public string? Server { get; set; }

        [Required(ErrorMessage = "O nome do banco de dados do legado é obrigatório")]
        [Display(Name = "Banco de dados do sistema legado")]
        public string? LegacyDatabaseName { get; set; }

        [Required(ErrorMessage = "A categoria padrão dos produtos é obrigatória")]
        [Display(Name = "Categoria padrão dos produtos")]
        public int DefaultCategoryId { get; set; }

        [Required(ErrorMessage = "A marca padrão dos produtos é obrigatória")]
        [Display(Name = "Marca padrão dos produtos")]
        public int DefaultBrandId { get; set; }

        [Required(ErrorMessage = "A loja é obrigatória")]
        [Display(Name = "Importar dados para qual loja?")]
        public int DefaultStoreId { get; set; }

        [Required(ErrorMessage = "O vendedor padrão é obrigatório")]
        [Display(Name = "Associar vendas a qual vendedor?")]
        public int DefaultSellerId { get; set; }
    }
}
