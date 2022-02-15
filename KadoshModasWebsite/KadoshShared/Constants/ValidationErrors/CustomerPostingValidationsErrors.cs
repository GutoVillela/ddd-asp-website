namespace KadoshShared.Constants.ValidationErrors
{
    public class CustomerPostingValidationsErrors
    {
        public const string NULL_CUSTOMER_ID = "O campo ID do cliente está nulo";
        public const string NULL_POSTING_TYPE = "O campo tipo de lançamento está nulo";
        public const string POSTING_VALUE_LOWER_THAN_OR_EQUALS_ZERO = "O campo valor do lançamento não pode ser menor ou igual a zero";
        public const string NULL_SALE_ID = "O campo ID da venda está nulo";
        public const string NULL_POSTING_DATE = "O campo data do lançamento está nulo";
    }
}
