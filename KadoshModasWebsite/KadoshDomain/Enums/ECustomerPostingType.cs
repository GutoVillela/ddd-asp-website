using System.ComponentModel;

namespace KadoshDomain.Enums
{
    public enum ECustomerPostingType
    {
        [Description("Compra à vista")]
        CashPurchase = 0,

        [Description("Pagamento de conta")]
        Payment = 1,

        [Description("Entrada")]
        DownPayment = 2,

        [Description("Estorno de pagamento")]
        ReversalPayment = 3,

        [Description("Devolução de item da compra")]
        SaleItemReturn = 4
    }
}
