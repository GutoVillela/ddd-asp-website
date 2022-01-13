using System.ComponentModel;

namespace KadoshDomain.Enums
{
    public enum EFormOfPayment
    {
        [Description("Dinheiro")]
        Cash = 0,

        [Description("Cartão de Débito")]
        DebitCard = 1,

        [Description("Cartão de Crédito")]
        CreditCard = 2,

        [Description("Cheque")]
        Check = 3
    }
}
