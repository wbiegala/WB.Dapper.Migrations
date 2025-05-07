using Dapper;
using WB.Dapper.Migrations.Core;

namespace WB.Dapper.Migrations.SqlServer.Core
{
    internal partial class SqlServerMigrationExecutedRepository : IMigrationExecutedRepository
    {
        private readonly ISqlConnectionProvider _connectionProvider;

        public SqlServerMigrationExecutedRepository(ISqlConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider
                ?? throw new ArgumentNullException(nameof(connectionProvider));
        }

        public async Task EnsureContextExistsAsync(CancellationToken cancellationToken = default)
        {
            using var connection = _connectionProvider.GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            var schemaCount = await connection.ExecuteScalarAsync<int>(CheckMigrationSchemaExistsQuery, transaction: transaction);

            if (schemaCount != 1)
            {
                await connection.ExecuteAsync(CreateMigrationSchemaQuery, transaction: transaction);
            }

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

            await connection.ExecuteAsync(InsertExecutedMigrationQuery, logEntry, transaction: transaction);

            await transaction.CommitAsync();
        }
    }
}
