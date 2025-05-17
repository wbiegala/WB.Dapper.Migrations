using Dapper;
using WB.Dapper.Migrations.Demo.Model;

namespace WB.Dapper.Migrations.Demo.Services
{
    internal class BookService : IBookService
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public BookService(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider
                ?? throw new ArgumentNullException(nameof(dbConnectionProvider));
        }

        public async Task<int?> AddBookAsync(AddBook book, CancellationToken cancellationToken = default)
        {
            using var connection = _dbConnectionProvider.GetConnection();
            await connection.OpenAsync(cancellationToken);

            using var transaction = await connection.BeginTransactionAsync(cancellationToken);

            try
            {
                var bookId = await connection.ExecuteScalarAsync<int?>(
                    InsertBookSql,
                    new { book.ISBN, book.Title, book.ReleaseYear },
                    transaction);

                foreach (var authorId in book.Authors)
                {
                    await connection.ExecuteAsync(
                        AssignAuthorsSql,
                        new { BookId = bookId, AuthorId = authorId },
                        transaction);
                }

                await transaction.CommitAsync(cancellationToken);

                return bookId;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<Book?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var connection = _dbConnectionProvider.GetConnection();
            await connection.OpenAsync(cancellationToken);

            var book = await connection.QuerySingleOrDefaultAsync<Book>(
                GetBookByIdSql,
                new { Id = id });

            if (book is null)
                return null;

            var authors = await connection.QueryAsync<Author>(
                GetAuhtorsByBookIdSql,
                new { BookId = id });

            book.Authors = authors;

            return book;
        }

        private const string BookTableName = "Books";
        private const string AuthorTableName = "Authors";
        private const string BookAuthorTableName = "BookAuthors";
        private const string InsertBookSql = @$"
INSERT INTO {BookTableName} (
    {nameof(Book.ISBN)},
    {nameof(Book.Title)},
    {nameof(Book.ReleaseYear)}
) VALUES (
    @{nameof(Book.ISBN)},
    @{nameof(Book.Title)},
    @{nameof(Book.ReleaseYear)}
) RETURNING {nameof(Book.Id)};
";

        private const string AssignAuthorsSql = @$"
INSERT INTO {BookAuthorTableName} (BookId, AuthorId) VALUES (@BookId, @AuthorId);
";

        private const string GetAuhtorsByBookIdSql = $@"
SELECT 
     au.Id         {nameof(Author.Id)}
    ,FirstName  {nameof(Author.FirstName)}
    ,LastName   {nameof(Author.LastName)}
    ,Born       {nameof(Author.Born)}
    ,Died       {nameof(Author.Died)}
FROM {AuthorTableName} au
INNER JOIN {BookAuthorTableName} ba ON au.Id = ba.AuthorId
WHERE ba.BookId = @BookId";

        private const string GetBookByIdSql = $@"
SELECT 
     Id        {nameof(Book.Id)}
    ,ISBN      {nameof(Book.ISBN)}
    ,Title     {nameof(Book.Title)}
    ,ReleaseYear {nameof(Book.ReleaseYear)}
FROM {BookTableName} b
WHERE b.Id = @Id";
    }
}
