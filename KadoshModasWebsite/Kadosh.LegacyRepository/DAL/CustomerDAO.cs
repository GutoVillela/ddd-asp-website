using KadoshDomain.Enums;
using KadoshDomain.LegacyEntities;
using KadoshDomain.ValueObjects;
using System.Data;
using System.Data.SqlClient;

namespace Kadosh.LegacyRepository.DAL
{
    /// <summary>
    /// Data Access Object for Customer Legacy table.
    /// </summary>
    internal class CustomerDAO
    {
        private const string TABLE_NAME = "TB_CLIENTES";
        private readonly Connection _connection;

        public CustomerDAO(Connection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<CustomerLegacy>> ReadAllAsync()
        {
            using var connection = await _connection.GetConnectionAsync();
            SqlCommand cmd = new(@"SELECT C.ID_CLIENTE, C.NOME, C.EMAIL, C.CPF, C.SEXO, C.ATIVO, C.DT_CRIACAO, C.DT_ATUALIZACAO, C.ATIVO, E.RUA, E.NUMERO, E.BAIRRO, CI.NOME AS CIDADE, ES.NOME AS ESTADO, E.COMPLEMENTO, E.CEP FROM " + TABLE_NAME + " C LEFT JOIN TB_ENDERECOS E ON C.ENDERECO = E.ID_ENDERECO LEFT JOIN TB_CIDADES CI ON CI.ID_CIDADE = E.CIDADE LEFT JOIN TB_ESTADOS ES ON ES.ID_ESTADO = CI.UF", connection);
            SqlDataReader dataReader = await cmd.ExecuteReaderAsync();

            List<CustomerLegacy> customers = new();

            while (await dataReader.ReadAsync())
            {
                Address? address = ReadAddressFromDataReader(dataReader);
                Document? document = ReadDocumentFromDataReader(dataReader);
                Email? email = ReadEmailFromDataReader(dataReader);

                CustomerLegacy customer = new(name: dataReader["NOME"].ToString() ?? string.Empty)
                {
                    Id = int.Parse(dataReader["ID_CLIENTE"].ToString()),
                    CreatedAt = DateTime.Parse(dataReader["DT_CRIACAO"].ToString()),
                    UpdatedAt = DateTime.Parse(dataReader["DT_ATUALIZACAO"].ToString()),
                    IsActive = bool.Parse(dataReader["ATIVO"].ToString()),
                    Gender = GetGenderFromLegacy(dataReader["SEXO"].ToString()),
                    Address = address,
                    Document = document,
                    Email = email

                };
                customers.Add(customer);
            }

            dataReader.Close();

            // TODO Improve efficiency on Read Customers Phone
            foreach(var customer in customers)
            {
                var phones = await ReadAllPhonesFromCustomerAsync(customer.Id, connection);
                customer.SetPhones(phones);
            }

            return customers;
        }

        private Address? ReadAddressFromDataReader(SqlDataReader dataReader)
        {
            Address? address = null;

            if (!string.IsNullOrEmpty(dataReader["RUA"].ToString()))
            {
                address = new(
                    street: dataReader["RUA"].ToString(),
                    number: dataReader["NUMERO"].ToString(),
                    neighborhood: dataReader["BAIRRO"].ToString(),
                    city: dataReader["CIDADE"].ToString(),
                    state: dataReader["ESTADO"].ToString(),
                    zipCode: dataReader["CEP"].ToString(),
                    complement: dataReader["COMPLEMENTO"].ToString()
                );
            }
            return address;
        }

        private Document? ReadDocumentFromDataReader(SqlDataReader dataReader)
        {
            Document? document = null;

            if (!string.IsNullOrEmpty(dataReader["CPF"].ToString()))
            {
                document = new(
                    number: dataReader["CPF"].ToString(),
                    type: EDocumentType.CPF
                );
            }
            return document;
        }

        private Email? ReadEmailFromDataReader(SqlDataReader dataReader)
        {
            Email? email = null;

            if (!string.IsNullOrEmpty(dataReader["EMAIL"].ToString()))
            {
                email = new(
                    emailAddress: dataReader["EMAIL"].ToString()
                );
            }
            return email;
        }

        private EGender GetGenderFromLegacy(string value)
        {
            switch (value)
            {
                case "0":
                    return EGender.Female;
                case "1":
                    return EGender.Male;
                default:
                    return EGender.NotDefined;
            }
        }

        private async Task<ICollection<Phone>> ReadAllPhonesFromCustomerAsync(int customerId, SqlConnection connection)
        {
            SqlCommand cmd = new(@"SELECT T.DDD, T.NUMERO, T.TIPO_TELEFONE, T.FALAR_COM FROM TB_TELEFONES_DO_CLIENTE TC INNER JOIN TB_CLIENTES C ON TC.CLIENTE = C.ID_CLIENTE INNER JOIN TB_TELEFONES T ON T.ID_TELEFONE = TC.TELEFONE WHERE C.ID_CLIENTE = @ID_CLIENTE", connection);
            cmd.Parameters.AddWithValue("@ID_CLIENTE", customerId).SqlDbType = SqlDbType.Int;

            SqlDataReader dataReader = await cmd.ExecuteReaderAsync();

            List<Phone> phones = new();

            while (await dataReader.ReadAsync())
            {
                Phone phone = new(
                    areaCode: dataReader["DDD"].ToString() ?? string.Empty,
                    number: dataReader["NUMERO"].ToString() ?? string.Empty,
                    type: GetPhoneTypeFromLegacy(dataReader["TIPO_TELEFONE"].ToString() ?? string.Empty),
                    talkTo: dataReader["FALAR_COM"].ToString() ?? string.Empty
                    );
                phones.Add(phone);
            }

            dataReader.Close();

            return phones;
        }

        private EPhoneType GetPhoneTypeFromLegacy(string value)
        {
            switch (value)
            {
                case "0":
                    return EPhoneType.Residential;
                case "1":
                    return EPhoneType.Commercial;
                case "2":
                    return EPhoneType.WhatsApp;
                case "3":
                    return EPhoneType.RelativeNumber;
                case "4":
                    return EPhoneType.AcquaintanceNumber;
                case "5":
                    return EPhoneType.Other;
                default:
                    return EPhoneType.Other;
            }
        }
    }
}
