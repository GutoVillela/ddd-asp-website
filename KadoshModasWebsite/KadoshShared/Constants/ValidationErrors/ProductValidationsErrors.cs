namespace KadoshShared.Constants.ValidationErrors
{
    public class ProductValidationsErrors
    {
        public const string INVALID_PRODUCT_ID = "O campo ID do produto está inválido";
        public const string INVALID_PRODUCT_NAME = "O campo nome do produto está inválido";
        public const string INVALID_PRODUCT_PRICE = "O campo preço do produto está inválido";
        public const string PRODUCT_PRICE_IS_ZERO = "O campo preço do produto precisa ser maior do que zero";
        public const string INVALID_PRODUCT_BRAND = "O campo marca do produto está inválido";
        public const string INVALID_PRODUCT_CATEGORY = "O campo categoria do produto está inválido";
        public const string QUERY_CURRENT_PAGE_LOWER_THAN_ZERO = "A página atual da consulta paginada não pode ser menor do que zero";
        public const string QUERY_PAGE_SIZE_LOWER_THAN_ZERO = "O tamanho da página da consulta paginada não pode ser menor do que zero";
        public const string PRODUCT_NAME_NOT_GIVEN_IN_QUERY = "O campo nome do produto é obrigatório para buscar os produtos por nome";
        public const string PRODUCT_BARCODE_NOT_GIVEN_IN_QUERY = "O campo Código de Barras é obrigatório para buscar o produto por código de barras";
    }
}
