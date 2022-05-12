using KadoshDomain.LegacyEntities;
using System.Data.SqlClient;

namespace Kadosh.LegacyRepository.DAL
{
    /// <summary>
    /// Data Access Object for Product Legacy table.
    /// </summary>
    internal class ProductDAO
    {
        private const string TABLE_NAME = "TB_PRODUTOS";
        private readonly Connection _connection;

        public ProductDAO(Connection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<ProductLegacy>> ReadAllAsync()
        {
            using var connection = await _connection.GetConnectionAsync();
            SqlCommand cmd = new(@"SELECT * FROM " + TABLE_NAME, connection);
            SqlDataReader dataReader = await cmd.ExecuteReaderAsync();

            List<ProductLegacy> products = new();

            while (await dataReader.ReadAsync())
            {
                ProductLegacy product = new(name: dataReader["NOME"].ToString() ?? string.Empty, price: decimal.Parse(dataReader["PRECO"].ToString() ?? "0"))
                {
                    Id = int.Parse(dataReader["ID_PRODUTO"].ToString() ?? "0"),
                    BarCode = dataReader["CODIGO_DE_BARRA"].ToString(),
                    Category = dataReader["CATEGORIA"].ToString(),
                    Brand = dataReader["MARCA"].ToString(),
                    CreatedAt = DateTime.Parse(dataReader["DT_CRIACAO"].ToString()),
                    UpdatedAt = DateTime.Parse(dataReader["DT_ATUALIZACAO"].ToString()),
                    IsActive = bool.Parse(dataReader["ATIVO"].ToString() ?? "true"),
                };
                products.Add(product);
            }

            dataReader.Close();

            return products;
        }
    }
}
