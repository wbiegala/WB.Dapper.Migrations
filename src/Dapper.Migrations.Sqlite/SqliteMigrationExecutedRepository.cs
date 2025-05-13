using Dapper;
using WB.Dapper.Migrations.Shared;

namespace WB.Dapper.Migrations.Sqlite
{
    internal partial class SqliteMigrationExecutedRepository : IMigrationExecutedRepository
    {
        private readonly ISqlConnectionProvider _connectionProvider;

        public SqliteMigrationExecutedRepository(ISqlConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider
                ?? throw new ArgumentNullException(nameof(connectionProvider));
        }

        public async Task EnsureContextExistsAsync(CancellationToken cancellationToken = default)
        {
            using var connection = _connectionProvider.GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            var tableCount = await connection.ExecuteScalarAsync<int>(CheckMigrationTableExistsQuery, transaction: transaction);

            if (tableCount == 1)
                return;

            await connection.ExecuteAsync(CreateMigrationTableQuery, transaction: transaction);

            await transaction.CommitAsync();
        }

        public async Task<IEnumerable<MigrationExecuted>> GetExecutedMigrationsAsync(CancellationToken cancellationToken = default)
        {
            using var connection = _connectionProvider.GetConnection();
            await connection.OpenAsync();
            var migrations = await connection.QueryAsync<MigrationExecuted>(GetExecutedMigrationsQuery);

            return migrations ?? Enumerable.Empty<MigrationExecuted>();
        }

        public async Task SaveAsync(MigrationExecuted logEntry, CancellationToken cancellationToken = default)
        {
            using var connection = _connectionProvider.GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            var logEntryWithId = new MigrationExecuted
            {
                Id = Guid.NewGuid(),
                Source = logEntry.Source,
                Number = logEntry.Number,
                Name = logEntry.Name,
                Describtion = logEntry.Describtion,
                Timestamp = logEntry.Timestamp
            };

            await connection.ExecuteAsync(InsertExecutedMigrationQuery, logEntryWithId, transaction: transaction);

            await transaction.CommitAsync();
        }
    }
}
