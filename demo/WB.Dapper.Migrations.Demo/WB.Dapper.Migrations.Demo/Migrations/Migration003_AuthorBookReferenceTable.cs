using Dapper;
using WB.Dapper.Migrations.Contract;

namespace WB.Dapper.Migrations.Demo.Migrations
{
    [Migration(3, "Create AuthorBookReference table", "Create AuthorBookReference table")]
    internal sealed class Migration003_AuthorBookReferenceTable : IMigration
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public Migration003_AuthorBookReferenceTable(IDbConnectionProvider dbConnectionProvider)
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
CREATE TABLE BookAuthors (
     Id         INTEGER PRIMARY KEY AUTOINCREMENT
    ,BookId     INTEGER NOT NULL
    ,AuthorId   INTEGER NOT NULL
    ,FOREIGN KEY (BookId) REFERENCES Books(Id)
    ,FOREIGN KEY (AuthorId) REFERENCES Authors(Id)
)
";
    }
}
