using KadoshDomain.Enums;
using System.ComponentModel.DataAnnotations;

namespace KadoshWebsite.Models
{
    public class PhoneViewModel
    {
        public int? Id { get; set; }

        public EPhoneType PhoneType { get; set; }

        public string PhoneNumber { get; set; }

        public string? TalkTo { get; set; }
    }
}
