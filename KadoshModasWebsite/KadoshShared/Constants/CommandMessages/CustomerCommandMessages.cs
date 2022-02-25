namespace KadoshShared.Constants.CommandMessages
{
    public class CustomerCommandMessages
    {
        public const string INVALID_CUSTOMER_CREATE_COMMAND = "Não foi possível cadastrar o cliente";
        public const string SUCCESS_ON_CREATE_CUSTOMER_COMMAND = "Cliente cadastrado com sucesso";
        public const string INVALID_CUSTOMER_UPDATE_COMMAND = "Não foi possível editar o cliente";
        public const string SUCCESS_ON_CUSTOMER_UPDATE_COMMAND = "Cliente editado com sucesso";
        public const string INVALID_CUSTOMER_DELETE_COMMAND = "Não foi possível apagar o cliente";
        public const string SUCCESS_ON_CUSTOMER_DELETE_COMMAND = "Cliente apagado com sucesso";
        public const string ERROR_CUSTOMER_NOT_FOUND = "Cliente não encontrado";
        public const string INVALID_CUSTOMER_INFORM_PAYMENT_COMMAND = "Não foi possível informar o pagamento do cliente";
        public const string SUCCESS_ON_INFORM_PAYMENT_COMMAND = "Pagamento registrado com sucesso";
        public const string SUCCESS_ON_INFORM_PAYMENT_COMMAND_WITH_RESERVATIONS = "Pagamento registrado com ressalvas. Do total de {0} apenas {1} foi necessário para quitar todas as dívidas do cliente";
        public const string ERROR_NO_OPEN_SALES_FOUND_TO_CUSTOMER = "Este cliente não possui nenhuma dívida em aberto";
        public const string ERROR_ONE_OR_MORE_INVALID_POSTINGS_TO_INFORMING_PAYMENT = "Não foi possível realizar um ou mais lançamentos nesse informe de pagamento. A operação foi abortada";
    }
}
