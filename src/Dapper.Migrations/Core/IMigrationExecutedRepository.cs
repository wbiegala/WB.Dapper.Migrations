namespace WB.Dapper.Migrations.Core
{
    public interface IMigrationExecutedRepository
    {
        Task EnsureContextExistsAsync(CancellationToken cancellationToken = default);
        Task SaveAsync(MigrationExecuted logEntry, CancellationToken cancellationToken = default);
        Task<IEnumerable<MigrationExecuted>> GetExecutedMigrationsAsync(CancellationToken cancellationToken = default);
    }
}
