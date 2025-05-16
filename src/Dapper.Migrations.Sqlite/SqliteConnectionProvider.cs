using Microsoft.Data.Sqlite;
using System.Data.Common;
using WB.Dapper.Migrations.Shared;

namespace WB.Dapper.Migrations.Sqlite
{
    internal sealed class SqliteConnectionProvider : ISqlConnectionProvider
    {
        private readonly string _connectionString;

        public SqliteConnectionProvider(string connectionString)
        {
            _connectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public DbConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}
