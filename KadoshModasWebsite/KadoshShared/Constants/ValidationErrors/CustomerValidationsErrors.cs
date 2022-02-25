namespace KadoshShared.Constants.ValidationErrors
{
    public class CustomerValidationsErrors
    {
        public const string INVALID_CUSTOMER_ID = "O campo ID do cliente está inválido";
        public const string INVALID_CUSTOMER_NAME = "O campo nome do cliente está inválido";
        public const string NULL_AMOUNT_TO_INFORM = "O valor é obrigatório para informar pagamento na ficha";
        public const string AMOUNT_TO_INFORM_LESS_OR_EQUALS_ZERO = "O valor deve ser maior do que zero para informar pagamento na ficha";
    }
}