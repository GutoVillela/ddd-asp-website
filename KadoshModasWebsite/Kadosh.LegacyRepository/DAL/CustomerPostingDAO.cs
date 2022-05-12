using KadoshDomain.Enums;
using KadoshDomain.LegacyEntities;
using System.Data;
using System.Data.SqlClient;

namespace Kadosh.LegacyRepository.DAL
{
    internal class CustomerPostingDAO
    {
        private const string TABLE_NAME = "TB_LANCAMENTOS_DO_CLIENTE";
        private readonly Connection _connection;

        public CustomerPostingDAO(Connection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<CustomerPostingLegacy>> ReadAllAsync()
        {
            using var connection = await _connection.GetConnectionAsync();
            SqlCommand cmd = new(@"SELECT * FROM " + TABLE_NAME, connection);
            
            SqlDataReader dataReader = await cmd.ExecuteReaderAsync();

            List<CustomerPostingLegacy> salePostings = new();

            while (await dataReader.ReadAsync())
            {
                CustomerPostingLegacy posting = new(
                    type: (ECustomerPostingLegacyType)Convert.ToInt32(dataReader["TIPO_LANCAMENTO_DO_CLIENTE"]),
                    value: Convert.ToDecimal(dataReader["VALOR_LANCAMENTO"]),
                    saleId: string.IsNullOrEmpty(dataReader["VENDA"].ToString()) ? 0 : Convert.ToInt32(dataReader["VENDA"]),
                    customerId: Convert.ToInt32(dataReader["CLIENTE"]),
                    postingDate: Convert.ToDateTime(dataReader["DT_LANCAMENTO"])
                    );

                salePostings.Add(posting);
            }

            dataReader.Close();

            return salePostings;
        }
    }
}
