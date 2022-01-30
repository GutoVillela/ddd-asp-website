using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.Enums
{
    public enum EUserRole
    {
        [Display(Name = "Administrador")]
        Administrator = 0,

        [Display(Name = "Vendedor")]
        Seller = 1
    }
}
