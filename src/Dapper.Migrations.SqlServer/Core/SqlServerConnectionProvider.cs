using Microsoft.Data.SqlClient;
using WB.Dapper.Migrations.Core;

namespace WB.Dapper.Migrations.SqlServer.Core
{
    internal class SqlServerConnectionProvider : ISqlConnectionProvider
    {
        private readonly string _connectionString;

        public SqlServerConnectionProvider(string connectionString)
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
