using System.Data;
using System.Data.SqlClient;

namespace Kadosh.LegacyRepository.DAL
{
    public class Connection :IDisposable
    {
        private readonly SqlConnection _connection;

        public Connection(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public async Task<SqlConnection> GetConnectionAsync()
        {
            if(_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();

            return _connection;
        }

        public void Dispose()
        {
            if(_connection != null && _connection.State == ConnectionState.Open)
                _connection.Close();
        }
    }
}
