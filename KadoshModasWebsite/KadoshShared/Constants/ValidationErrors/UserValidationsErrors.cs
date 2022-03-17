namespace KadoshShared.Constants.ValidationErrors
{
    public class UserValidationsErrors
    {
        public const string INVALID_USER_ID = "O campo ID do usuário está inválido";
        public const string INVALID_USER_NAME = "O campo nome está inválido";
        public const string INVALID_USER_USERNAME = "O campo nome de usuário está inválido";
        public const string INVALID_USER_PASSWORD = "O campo senha está inválido";
        public const string INVALID_USER_ROLE = "O campo cargo de usuário está inválido";
        public const string INVALID_USER_STORE = "O campo loja do usuário está inválido";
        public const string INVALID_USER_ORIGINAL_USERNAME = "O nome de usuário original está inválido";
        public const string QUERY_CURRENT_PAGE_LOWER_THAN_ZERO = "A página atual da consulta paginada não pode ser menor do que zero";
        public const string QUERY_PAGE_SIZE_LOWER_THAN_ZERO = "O tamanho da página da consulta paginada não pode ser menor do que zero";
    }
}
