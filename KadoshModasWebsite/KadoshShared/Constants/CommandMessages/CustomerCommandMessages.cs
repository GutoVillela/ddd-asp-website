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
        public const string INVALID_CUSTOMER_AUTHENTICATE_COMMAND = "O comando para autenticar o usuário do cliente está inválido";
        public const string ERROR_CUSTOMER_USERNAME_NOT_FOUND = "Não foi encontrado um cliente para o usuário informado";
        public const string ERROR_CUSTOMER_USER_PASSWORD_NOT_SET = "A senha do cliente não está definida corretamente. Por favor altere a senha e tente novamente";
        public const string ERROR_AUTHENTICATION_FAILED = "O nome de usuário ou senha estão incorretos";
        public const string SUCCESS_ON_AUTHENTICATE_CUSTOMER_USER_COMMAND = "Usuário do cliente autenticado com sucesso";
        public const string INVALID_CUSTOMER_USER_CREATE_COMMAND = "Não foi possível cadastrar o usuário do cliente";
        public const string SUCCESS_ON_CREATE_CUSTOMER_USER_COMMAND = "Usuário cadastrado com sucesso";
        public const string ERROR_USERNAME_ALREADY_TAKEN = "O nome de usuário já está sendo usado";
        public const string ERROR_CUSTOMER_USER_ALREADY_CREATED = "Usuário já criado para o cliente";
        public const string INVALID_MERGE_CUSTOMERS_COMMAND = "O comando de mesclar clientes está inválido";
        public const string SUCCESS_ON_MERGE_CUSTOMER_COMMAND = "Clientes mesclados com sucesso";
        public const string ERROR_CUSTOMER_ALREADY_BOUNDED = "O cliente já está mesclado";
        public const string ERROR_CUSTOMER_HAS_BOUNDED_CUSTOMERS = "O cliente {0} (#{1}) possui clientes mesclados. Não é possível mesclá-lo a outra ficha";
    }
}
