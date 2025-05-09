using Microsoft.Data.SqlClient;
using WB.Dapper.Migrations.Shared;

namespace WB.Dapper.Migrations.Core
{
    internal class BasicSqlConnectionProvider : ISqlConnectionProvider
    {
        private readonly string _connectionString;

        public BasicSqlConnectionProvider(string connectionString)
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
