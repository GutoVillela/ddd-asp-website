namespace KadoshShared.Constants.ValidationErrors
{
    public class StoreValidationErrors
    {
        public const string INVALID_STORE_NAME_ERROR = "Nome da loja inválido";
        public const string INVALID_STORE_STREET = "O campo rua da loja está inválido";
        public const string INVALID_STORE_NUMBER = "O campo número da loja está inválido";
        public const string INVALID_STORE_ID = "O campo ID da loja está inválido";
        public const string QUERY_CURRENT_PAGE_LOWER_THAN_ZERO = "A página atual da consulta paginada não pode ser menor do que zero";
        public const string QUERY_PAGE_SIZE_LOWER_THAN_ZERO = "O tamanho da página da consulta paginada não pode ser menor do que zero";
    }
}
