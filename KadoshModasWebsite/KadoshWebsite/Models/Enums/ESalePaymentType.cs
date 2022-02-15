using System.ComponentModel.DataAnnotations;

namespace KadoshWebsite.Models.Enums
{
    public enum ESalePaymentType
    {
        [Display(Name = "À Vista")]
        Cash = 0,

        [Display(Name = "Parcelado")]
        InStallments = 1,

        [Display(Name = "Fiado")]
        OnCredit = 2,
    }
}
