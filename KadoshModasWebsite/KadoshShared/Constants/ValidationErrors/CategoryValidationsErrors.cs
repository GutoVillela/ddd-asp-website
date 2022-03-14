namespace KadoshShared.Constants.ValidationErrors
{
    public class CategoryValidationsErrors
    {
        public const string INVALID_CATEGORY_ID = "O campo ID da categoria está inválido";
        public const string INVALID_CATEGORY_NAME = "O campo nome da categoria está inválido";
        public const string QUERY_CURRENT_PAGE_LOWER_THAN_ZERO = "A página atual da consulta paginada não pode ser menor do que zero";
        public const string QUERY_PAGE_SIZE_LOWER_THAN_ZERO = "O tamanho da página da consulta paginada não pode ser menor do que zero";
    }
}
