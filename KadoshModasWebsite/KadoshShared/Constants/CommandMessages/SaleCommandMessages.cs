namespace KadoshShared.Constants.CommandMessages
{
    public class SaleCommandMessages
    {
        public const string INVALID_SALE_IN_CASH_CREATE_COMMAND = "Não foi possível cadastrar a compra em dinheiro";
        public const string INVALID_SALE_IN_INSTALLMENTS_CREATE_COMMAND = "Não foi possível cadastrar a compra parcela";
        public const string INVALID_SALE_ON_CREDIT_CREATE_COMMAND = "Não foi possível cadastrar a compra fiado";
        public const string SUCCESS_ON_CREATE_SALE_COMMAND = "Venda registrada com sucesso";
        public const string ERROR_COULD_NOT_FIND_SALE_CUSTOMER = "Não foi possível encontrar o cliente associado à venda";
        public const string ERROR_COULD_NOT_FIND_SALE_SELLER = "Não foi possível encontrar o vendedor associado à venda";
        public const string ERROR_COULD_NOT_FIND_SALE_STORE = "Não foi possível encontrar a loja associado à venda";
        public const string ERROR_COULD_NOT_FIND_SALE_PRODUCT = "Não foi possível encontrar um ou mais produtos da venda";
        public const string ERROR_INVALID_SALE_ITEM = "Um ou mais itens da venda estão inválidos";
        public const string ERROR_COULD_NOT_CREATE_SALE_IN_CASH_POSTING = "Não foi possível realizar o lançamento da venda em à vista";
        public const string ERROR_COULD_NOT_CREATE_DOWN_PAYMENT_POSTING = "Não foi possível realizar o lançamento da entrada da venda";
        public const string ERROR_INVALID_SALE_INSTALLMENT = "Uma ou mais parcelas da venda estão inválidas";
        public const string UNEXPECTED_EXCEPTION = "Uma exceção inesperada aconteceu";
        public const string INVALID_PAYOFF_SALE_COMMAND = "Não foi possível quitar a dívida";
        public const string ERROR_COULD_NOT_FIND_SALE = "Não foi possível encontrar a venda para o ID informado";
        public const string ERROR_COULD_NOT_CREATE_SALE_PAYOFF_POSTING = "Não foi possível realizar o lançamento da quitação da venda";
        public const string SUCCESS_ON_SALE_PAYOFF_COMMAND = "Venda quitada com sucesso";
        public const string INVALID_SALE_INFORM_PAYMENT_COMMAND = "Não foi possível informar o pagamento da venda";
        public const string SUCCESS_ON_INFORM_SALE_PAYMENT_COMMAND = "Pagamento registrado com sucesso";
        public const string THERE_IS_NO_VALUE_TO_INFORM_ON_INFORM_SALE_PAYMENT_COMMAND = "A venda não possui valor para ser lançado";
        public const string INVALID_SITUATION_ON_INFORM_SALE_PAYMENT_COMMAND = "A venda está concluída ou cancelada. Só é possível informar pagamentos em vendas em aberto";
        public const string SUCCESS_ON_INFORM_SALE_PAYMENT_COMMAND_PAYOFF = "Pagamento registrado com sucesso. A venda foi quitada. Foi realizado um lançamento no valor de {0}";
        public const string ERROR_INVALID_POSTING_TO_INFORMING_PAYMENT = "Não foi possível realizar o informe de pagamento. A operação foi abortada";
        public const string ERROR_CANNOT_INFORM_PAYMENT_TO_SALE_IN_INSTALLMENTS = "Não é possível informar um pagamento personalizado em uma venda parcelada. É necessário quitar as parcelas manualmente ou quitar a venda.";
        public const string INVALID_PAYOFF_INSTALLMENT_COMMAND = "Comando de informar pagamento de parcela inválido";
        public const string INVALID_PAYOFF_INSTALLMENT_SALE = "A venda {0} não é do tipo Parcelado e portanto não é possível informar pagamento de parcela para ela";
        public const string ERROR_COULD_NOT_FIND_SALE_INSTALLMENT = "Não foi possível encontrar a parcela associada à venda";
        public const string INVALID_SALE_SITUATION_ON_PAYOFF_INSTALLMENT_COMMAND = "A venda está concluída ou cancelada. Só é possível informar pagamentos da parcelas em vendas em aberto";
        public const string INVALID_INSTALLMENT_SITUATION_ON_PAYOFF_INSTALLMENT_COMMAND = "A parcela já está quitada";
        public const string ERROR_INVALID_POSTING_TO_PAYOFF_INSTALLMENT = "Lançamento para quitação de parcela inválido";
        public const string SUCCESS_ON_PAYOFF_INSTALLMENT_COMMAND = "Parcela quitada com sucesso";
    }
}
