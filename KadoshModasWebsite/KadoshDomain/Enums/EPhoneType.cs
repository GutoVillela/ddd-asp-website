using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.Enums
{
    public enum EPhoneType
    {
        [Display(Name = "Residencial")]
        Residential = 0,

        [Display(Name = "Comercial")]
        Commercial = 1,

        [Display(Name = "WhatsApp")]
        WhatsApp = 2,

        [Display(Name = "Número de parente")]
        RelativeNumber = 3,

        [Display(Name = "Número de conhecido")]
        AcquaintanceNumber = 4,

        [Display(Name = "Outro")]
        Other = 5
    }
}
