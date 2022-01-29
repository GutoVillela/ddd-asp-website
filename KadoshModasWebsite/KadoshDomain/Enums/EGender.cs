using KadoshShared.Constants.Descriptions;
using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.Enums
{
    public enum EGender
    {
        [Display(Name = EGenderDescriptions.NOT_DEFINED_DESCRIPTION)]
        NotDefined = 0,

        [Display(Name = EGenderDescriptions.MALE_DESCRIPTION)]
        Male = 1,

        [Display(Name = EGenderDescriptions.FEMALE_DESCRIPTION)]
        Female = 2
    }
}
