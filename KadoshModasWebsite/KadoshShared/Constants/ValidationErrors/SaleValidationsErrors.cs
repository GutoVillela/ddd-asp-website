namespace KadoshShared.Constants.ValidationErrors
{
    public class SaleValidationsErrors
    {
        public const string INVALID_SALE_CUSTOMER = "O campo Cliente da Venda está inválido";
        public const string INVALID_SALE_FORM_OF_PAYMENT = "O campo Forma de Pagamento da Venda está inválido";
        public const string SALE_DISCOUNT_OUT_OF_RANGE = "O campo Desconto da Venda deve estar entre 0 e 100%";
        public const string DOWN_PAYMENT_LOWER_THAN_ZERO = "O campo Entrada da Venda não pode ser negativo";
        public const string NULL_SALE_DATE = "O campo Data da Venda é obrigatório";
        public const string NULL_SELLER_ID = "O campo Vendedor é obrigatório";
        public const string NULL_STORE_ID = "O campo Loja é obrigatório";
        public const string SETTLEMENT_DATE_LOWER_THAN_SALE_DATE = "A data de conclusão da venda não pode ser menor que a data da venda";
        public const string EMPTY_SALE_LIST_ITEMS = "É obrigatório pelo menos um item da venda para registrar a venda";
        public const string EMPTY_SALE_INSTALLMENTS = "Não é possível criar uma venda parcelada sem nenhuma parcela";
        public const string LESS_THAN_TWO_INSTALLMENTS_ERROR = "Não é possível criar uma venda parcelada sem pelo menos 2 parcelas";
        public const string NEGATIVE_INTEREST_ON_TOTAL_SALE_ERROR = "Não é possível criar uma venda parcelada com o juros total negativo";
        public const string NULL_SALE_ID = "O ID da venda é obrigatório";
    }
}
