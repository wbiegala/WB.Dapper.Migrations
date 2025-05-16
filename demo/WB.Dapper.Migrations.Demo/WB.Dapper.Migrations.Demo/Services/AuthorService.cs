using Dapper;
using WB.Dapper.Migrations.Demo.Model;

namespace WB.Dapper.Migrations.Demo.Services
{
    internal sealed class AuthorService : IAuthorService
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public AuthorService(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider
                ?? throw new ArgumentNullException(nameof(dbConnectionProvider));
        }

        public async Task<int?> AddAuthorAsync(AddAuthor author, CancellationToken cancellationToken = default)
        {
            using var connection = _dbConnectionProvider.GetConnection();
            await connection.OpenAsync(cancellationToken);

            using var transaction = await connection.BeginTransactionAsync(cancellationToken);
            var authorId = await connection.ExecuteScalarAsync<int?>(InsertAuthorSql, author, transaction);

            await transaction.CommitAsync();

            return authorId;
        }

        public async Task<Author?> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var connection = _dbConnectionProvider.GetConnection();
            await connection.OpenAsync(cancellationToken);

            return await connection.QuerySingleOrDefaultAsync<Author>(GetAuthorByIdSql, new { Id = id });
        }

        private const string AuthorTableName = "Authors";

        private const string InsertAuthorSql = @$"
INSERT INTO {AuthorTableName} (
    {nameof(Author.FirstName)},
    {nameof(Author.LastName)},
    {nameof(Author.Born)},
    {nameof(Author.Died)}
) VALUES (
    @{nameof(Author.FirstName)},
    @{nameof(Author.LastName)},
    @{nameof(Author.Born)},
    @{nameof(Author.Died)}
) RETURNING {nameof(Author.Id)};
";

        private const string GetAuthorByIdSql = $"SELECT * FROM {AuthorTableName} WHERE {nameof(Author.Id)} = @Id";
    }
}
