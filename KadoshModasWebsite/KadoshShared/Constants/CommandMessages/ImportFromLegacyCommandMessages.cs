namespace KadoshShared.Constants.CommandMessages
{
    public class ImportFromLegacyCommandMessages
    {
        public const string SUCCESS_ON_IMPORT_FROM_LEGACY_COMMAND = "Dados importados do legado com sucesso";
        public const string INVALID_IMPORT_FROM_LEGACY_COMMAND = "O comando de importação de dados do legado está inválido";
        public const string COULD_NOT_FIND_IMPORT_FROM_LEGACY_STORE = "A loja informada para importação dos dados do legado não foi encontrada";
        public const string COULD_NOT_FIND_IMPORT_FROM_LEGACY_SELLER = "O vendedor informado para importação dos dados do legado não foi encontrada";
        public const string COULD_NOT_FIND_IMPORT_FROM_LEGACY_BRAND = "A marca informada para importação dos dados do legado não foi encontrada";
        public const string COULD_NOT_FIND_IMPORT_FROM_LEGACY_CATEGORY = "A categoria informada para importação dos dados do legado não foi encontrada";
        public const string CUSTOMER_ID_NOT_FIND_FROM_IMPORTED_CUSTOMERS = "Não foi encontrado o ID de cliente para uma ou mais vendas importadas do legado";
        public const string PRODUCT_ID_NOT_FIND_FROM_IMPORTED_PRODUCTS = "Não foi encontrado o ID de produto para uma ou mais itens de venda importados do legado";
        public const string SALE_ID_NOT_FIND_FROM_IMPORTED_SALES = "Não foi encontrado o ID da venda para um ou mais lançamentos do cliente importados do legado";
        public const string CUSTOMER_ID_NOT_FIND_FROM_IMPORTED_CUSTOMER_POSTING = "Não foi encontrado o ID do cliente associado ao lançamento do cliente importado do legado";
        public const string UNEXPECTED_EXCEPTION_ON_IMPORT_FROM_LEGACY_COMMAND  = "Aconteceu um erro ao importar os dados do legado. Mensagem original: {0}";
    }
}