namespace KadoshShared.Constants.ValidationErrors
{
    public class CustomerPostingValidationsErrors
    {
        public const string NULL_CUSTOMER_ID = "O campo ID do cliente está nulo";
        public const string NULL_POSTING_TYPE = "O campo tipo de lançamento está nulo";
        public const string POSTING_VALUE_LOWER_THAN_OR_EQUALS_ZERO = "O campo valor do lançamento não pode ser menor ou igual a zero";
        public const string NULL_SALE_ID = "O campo ID da venda está nulo";
        public const string NULL_POSTING_DATE = "O campo data do lançamento está nulo";
        public const string QUERY_CURRENT_PAGE_LOWER_THAN_ZERO = "A página atual da consulta paginada não pode ser menor do que zero";
        public const string QUERY_PAGE_SIZE_LOWER_THAN_ZERO = "O tamanho da página da consulta paginada não pode ser menor do que zero";
        public const string NULL_QUERY_DATE = "O campo data está nulo para buscar os lançamentos";
        public const string NULL_QUERY_STORE_ID = "O campo ID da loja não pode estar nulo para buscar os lançamentos";
        public const string NULL_QUERY_TIMEZONE = "É necessário definir o fuso horário de destino para a busca dos lançamentos";
    }
}
