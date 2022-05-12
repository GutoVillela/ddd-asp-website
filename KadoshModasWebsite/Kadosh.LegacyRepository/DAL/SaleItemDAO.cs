using KadoshDomain.Enums;
using KadoshDomain.LegacyEntities;
using System.Data;
using System.Data.SqlClient;

namespace Kadosh.LegacyRepository.DAL
{
    internal class SaleItemDAO
    {
        private const string TABLE_NAME = "TB_ITENS_DA_VENDA";
        private readonly Connection _connection;

        public SaleItemDAO(Connection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<SaleItemLegacy>> ReadAllFromSaleLegacyAsync(int saleLegacyId)
        {
            using var connection = await _connection.GetConnectionAsync();
            SqlCommand cmd = new (@"SELECT * FROM " + TABLE_NAME + " WHERE VENDA = @SALE", connection);
            cmd.Parameters.AddWithValue("@SALE", saleLegacyId).SqlDbType = SqlDbType.Int;
            SqlDataReader dataReader = await cmd.ExecuteReaderAsync();

            List<SaleItemLegacy> saleItems = new();

            while (await dataReader.ReadAsync())
            {
                SaleItemLegacy saleItem = new()
                {
                    ProductId = int.Parse(dataReader["PRODUTO"].ToString()),
                    Amount = int.Parse(dataReader["QUANTIDADE"].ToString()),
                    Price = decimal.Parse(dataReader["VALOR_ITEM"].ToString()),
                    Discount = string.IsNullOrEmpty(dataReader["DESCONTO"].ToString()) ? 0 : decimal.Parse(dataReader["DESCONTO"].ToString()),
                    Situation = (ESaleItemLegacySituation)Convert.ToInt32(dataReader["SITUACAO_ITEM"]),
                    SaleId = int.Parse(dataReader["VENDA"].ToString())
                };

                saleItems.Add(saleItem);
            }

            dataReader.Close();

            return saleItems;
        }
    }
}
