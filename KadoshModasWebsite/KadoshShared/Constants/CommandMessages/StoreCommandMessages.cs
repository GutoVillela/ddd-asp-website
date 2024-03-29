﻿namespace KadoshShared.Constants.CommandMessages
{
    public class StoreCommandMessages
    {
        public const string INVALID_STORE_CREATE_COMMAND = "Não foi possível cadastrar a nova loja";
        public const string SUCCESS_ON_CREATE_STORE_COMMAND = "Loja cadastrada com sucesso";
        public const string INVALID_STORE_UPDATE_COMMAND = "Não foi possível atualizar a loja";
        public const string SUCCESS_ON_UPDATE_STORE_COMMAND = "Loja atualizada com sucesso";
        public const string INVALID_STORE_DELETE_COMMAND = "Não foi possível apagar a loja";
        public const string SUCCESS_ON_DELETE_STORE_COMMAND = "Loja apagada com sucesso";
        public const string COULD_NOT_FIND_STORE = "Não foi possível obter a loja do banco de dados";
        public const string REPEATED_STORE_ADDRESS = "Não foi possível salvar a loja pois já existe outra loja cadastrada no mesmo endereço";
        public const string ERROR_STORE_NOT_FOUND = "Não foi encontrada uma loja para o ID fornecido";

    }
}
