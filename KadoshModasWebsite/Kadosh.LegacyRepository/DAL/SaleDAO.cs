using KadoshDomain.Enums;
using KadoshDomain.LegacyEntities;
using System.Data.SqlClient;

namespace Kadosh.LegacyRepository.DAL
{
    /// <summary>
    /// Data Access Object for Sale Legacy table.
    /// </summary>
    internal class SaleDAO
    {
        private const string TABLE_NAME = "TB_VENDAS";
        private readonly Connection _connection;

        public SaleDAO(Connection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<SaleLegacy>> ReadAllAsync()
        {
            using var connection = await _connection.GetConnectionAsync();
            SqlCommand cmd = new(@"SELECT * FROM " + TABLE_NAME, connection);
            SqlDataReader dataReader = await cmd.ExecuteReaderAsync();

            List<SaleLegacy> sales = new();

            while (await dataReader.ReadAsync())
            {
                SaleLegacy sale = new(
                    customerId: int.Parse(dataReader["CLIENTE"].ToString()),
                    saleType: (ESaleLegacyType)int.Parse(dataReader["TIPO_PAGAMENTO"].ToString()),
                    discount: decimal.Parse(dataReader["DESCONTO"].ToString()),
                    total: decimal.Parse(dataReader["TOTAL"].ToString()),
                    downPayment: decimal.Parse(dataReader["ENTRADA"].ToString()),
                    situation: (ESaleLegacySituation)int.Parse(dataReader["SITUACAO"].ToString()),
                    formOfPayment: (ELegacyFormOfPayment)int.Parse(dataReader["FORMA_DE_PAGAMENTO"].ToString()),
                    saleDate: DateTime.Parse(dataReader["DT_VENDA"].ToString())
                    )
                {
                    Id = int.Parse(dataReader["ID_VENDA"].ToString()),
                    CreatedAt = DateTime.Parse(dataReader["DT_CRIACAO"].ToString()),
                    UpdatedAt = DateTime.Parse(dataReader["DT_ATUALIZACAO"].ToString())
                };

                if (!string.IsNullOrEmpty(dataReader["DT_QUITACAO"].ToString()))
                    sale.SettlementDate = DateTime.Parse(dataReader["DT_QUITACAO"].ToString());

                if (!string.IsNullOrEmpty(dataReader["PAGO"].ToString()))
                    sale.Paid = decimal.Parse(dataReader["PAGO"].ToString());

                sales.Add(sale);
            }

            dataReader.Close();

            return sales;
        }
    }
}