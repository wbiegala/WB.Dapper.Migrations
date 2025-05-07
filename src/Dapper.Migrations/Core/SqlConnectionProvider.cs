using Microsoft.Data.SqlClient;

namespace WB.Dapper.Migrations.Core
{
    internal class SqlConnectionProvider : ISqlConnectionProvider
    {
        private readonly string _connectionString;

        public SqlConnectionProvider(string connectionString)
        {
            _connectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
