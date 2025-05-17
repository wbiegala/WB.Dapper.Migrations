using Dapper;
using WB.Dapper.Migrations.Contract;
using WB.Dapper.Migrations.Demo.Model;

namespace WB.Dapper.Migrations.Demo.Migrations
{
    [Migration(1, "Create Author table", "Create Author table")]
    internal sealed class Migration001_AuthorTable : IMigration
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public Migration001_AuthorTable(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        public async Task MigrateAsync()
        {
            using var connection = _dbConnectionProvider.GetConnection();
            await connection.OpenAsync();

            await connection.ExecuteAsync(CreateTableSql);
        }

        private const string CreateTableSql = $@"
CREATE TABLE Authors (
    {nameof(Author.Id)}             INTEGER PRIMARY KEY AUTOINCREMENT
    ,{nameof(Author.FirstName)}      TEXT NOT NULL
    ,{nameof(Author.LastName)}       TEXT NOT NULL
    ,{nameof(Author.Born)}           TEXT NOT NULL
    ,{nameof(Author.Died)}           TEXT NULL
)";
    }
}
