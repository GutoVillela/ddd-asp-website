using KadoshDomain.LegacyEntities;
using System.Data.SqlClient;

namespace Kadosh.LegacyRepository.DAL
{
    /// <summary>
    /// Data Access Object for Category Legacy table.
    /// </summary>
    internal class CategoryDAO
    {
        private const string TABLE_NAME = "TB_CATEGORIAS";
        private readonly Connection _connection;

        public CategoryDAO(Connection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<CategoryLegacy>> ReadAllAsync()
        {
            using var connection = await _connection.GetConnectionAsync();
            SqlCommand cmd = new(@"SELECT * FROM " + TABLE_NAME, connection);
            SqlDataReader dataReader = await cmd.ExecuteReaderAsync();

            List<CategoryLegacy> categories = new();

            while (await dataReader.ReadAsync())
            {
                CategoryLegacy category = new(name: dataReader["NOME"].ToString() ?? string.Empty)
                {
                    CreatedAt = DateTime.Parse(dataReader["DT_CRIACAO"].ToString()),
                    UpdatedAt = DateTime.Parse(dataReader["DT_ATUALIZACAO"].ToString()),
                    IsActive = bool.Parse(dataReader["ATIVO"].ToString()),
                };
                categories.Add(category);
            }

            dataReader.Close();

            return categories;
        }
    }
}
