using Dapper;
using WB.Dapper.Migrations.Contract;
using WB.Dapper.Migrations.Demo.Model;

namespace WB.Dapper.Migrations.Demo.Migrations
{
    [Migration(2, "Create Book Table", "Create Book Table")]
    internal sealed class Migration002_BookTable : IMigration
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public Migration002_BookTable(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider
                ?? throw new ArgumentNullException(nameof(dbConnectionProvider));
        }

        public async Task MigrateAsync()
        {
            using var connection = _dbConnectionProvider.GetConnection();
            await connection.OpenAsync();

            await connection.ExecuteAsync(CreateTableSql);
        }

        private const string CreateTableSql = $@"
CREATE TABLE Books (
    {nameof(Book.Id)}             INTEGER PRIMARY KEY AUTOINCREMENT
    ,{nameof(Book.ISBN)}           TEXT NOT NULL
    ,{nameof(Book.Title)}          TEXT NOT NULL
    ,{nameof(Book.ReleaseYear)}    INTEGER NULL
)";
    }
}
