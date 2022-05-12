using KadoshDomain.LegacyEntities;
using System.Data.SqlClient;

namespace Kadosh.LegacyRepository.DAL
{
    /// <summary>
    /// Data Access Object for Brand Legacy table.
    /// </summary>
    internal class BrandDAO
    {
        private const string TABLE_NAME = "TB_MARCAS";
        private readonly Connection _connection;

        public BrandDAO(Connection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<BrandLegacy>> ReadAllAsync()
        {
            using var connection = await _connection.GetConnectionAsync();
            SqlCommand cmd = new(@"SELECT * FROM " + TABLE_NAME, connection);
            SqlDataReader dataReader = await cmd.ExecuteReaderAsync();

            List<BrandLegacy> brands = new();

            while (await dataReader.ReadAsync())
            {
                BrandLegacy brand = new(name: dataReader["NOME"].ToString() ?? string.Empty)
                {
                    CreatedAt = DateTime.Parse(dataReader["DT_CRIACAO"].ToString()),
                    UpdatedAt = DateTime.Parse(dataReader["DT_ATUALIZACAO"].ToString())
                };
                brands.Add(brand);
            }

            dataReader.Close();

            return brands;
        }
    }
}
