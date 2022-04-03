namespace KadoshShared.Constants.ValidationErrors
{
    public class CustomerValidationsErrors
    {
        public const string INVALID_CUSTOMER_ID = "O campo ID do cliente está inválido";
        public const string INVALID_CUSTOMER_NAME = "O campo nome do cliente está inválido";
        public const string NULL_AMOUNT_TO_INFORM = "O valor é obrigatório para informar pagamento na ficha";
        public const string AMOUNT_TO_INFORM_LESS_OR_EQUALS_ZERO = "O valor deve ser maior do que zero para informar pagamento na ficha";
        public const string QUERY_CURRENT_PAGE_LOWER_THAN_ZERO = "A página atual da consulta paginada não pode ser menor do que zero";
        public const string QUERY_PAGE_SIZE_LOWER_THAN_ZERO = "O tamanho da página da consulta paginada não pode ser menor do que zero";
        public const string QUERY_NULL_INTERVAL_SINCE_LAST_PAYMENT = "O intervalo em dias desde o último pagamento deve ser fornecido para buscar os clients inadimplentes";
        public const string QUERY_INTERVAL_SINCE_LAST_PAYMENT_LOWER_THAN_OR_EQUALS_ZERO = "O intervalo em dias desde o último pagamento deve ser maior do que zero para buscar os clients inadimplentes";
    }
}