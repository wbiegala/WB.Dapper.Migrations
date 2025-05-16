using WB.Dapper.Migrations.Demo.Model;

namespace WB.Dapper.Migrations.Demo.Services
{
    public interface IBookService
    {
        Task<int?> AddBookAsync(AddBook book, CancellationToken cancellationToken = default);
        Task<Book?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default);
    }

    public sealed record AddBook
    {
        public string ISBN { get; init; }
        public string Title { get; init; }
        public int ReleaseYear { get; init; }
        public IEnumerable<int> Authors { get; init; }
    }
}
