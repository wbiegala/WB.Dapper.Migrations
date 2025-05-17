using WB.Dapper.Migrations.Demo.Model;

namespace WB.Dapper.Migrations.Demo.Services
{
    public interface IAuthorService
    {
        Task<int?> AddAuthorAsync(AddAuthor author, CancellationToken cancellationToken = default);
        Task<Author?> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default);
    }

    public sealed record AddAuthor
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public DateTime Born { get; init; }
        public DateTime? Died { get; init; }
    }
}
