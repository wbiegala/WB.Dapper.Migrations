using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace WB.Dapper.Migrations.Demo
{
    static class Database
    {
        public const string ConnectionString = "Data Source=demo.db;Cache=Shared";
    }

    public interface IDbConnectionProvider
    {
        DbConnection GetConnection();
    }

    internal sealed class DbConnectionProvider : IDbConnectionProvider
    {
        private readonly string _connectionString = Database.ConnectionString;

        public DbConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}