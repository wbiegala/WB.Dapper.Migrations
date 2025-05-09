using WB.Dapper.Migrations.Contract;
using WB.Dapper.Migrations.Contract.Exceptions;

namespace WB.Dapper.Migrations.Core
{
    internal class MigrationExecutor : IMigrationExecutor
    {
        private readonly IMigrationProvider _migrationProvider;
        private readonly IMigrationExecutedRepository _migrationLogRepository;

        public MigrationExecutor(IMigrationProvider migrationProvider,
            IMigrationExecutedRepository migrationLogRepository)
        {
            _migrationProvider = migrationProvider
                ?? throw new ArgumentNullException(nameof(migrationProvider));
            _migrationLogRepository = migrationLogRepository
                ?? throw new ArgumentNullException(nameof(migrationLogRepository));
        }

        public async Task MigrateDatabaseAsync()
        {
            try
            {
                await _migrationLogRepository.EnsureContextExistsAsync();
            }
            catch (Exception ex)
            {
                throw new MigrationException($"Error while ensuring that migration data structure exists in given database. Reason: {ex.Message}", ex);
            }

            var migrations = _migrationProvider.GetMigrations();
            var migrationsBySource = migrations.GroupBy(k => k.MigrationSource, v => v);
            var migrationsExecuted = await _migrationLogRepository.GetExecutedMigrationsAsync();

            foreach (var group in migrationsBySource)
            {
                var sorted = group.OrderBy(m => m.Number);

                foreach (var migration in sorted)
                {
                    var isMigrationDoneAlready = migrationsExecuted.Any(me => me.Source == migration.MigrationSource
                            && me.Number == migration.Number);

                    if (isMigrationDoneAlready)
                        continue;

                    try
                    {
                        await migration.MigrateAsync();
                        await NotifyMigrationExecutionAsync(migration);
                    }
                    catch (Exception ex)
                    {
                        throw new MigrationException(GetMigrationError(migration, ex), ex);
                    }
                }
            }
        }

        private async Task NotifyMigrationExecutionAsync(MigrationContext context)
        {
            var migrationLog = new MigrationExecuted
            {
                Source = context.MigrationSource,
                Number = context.Number,
                Name = context.Name,
                Describtion = context.Describtion,
                Timestamp = DateTimeOffset.UtcNow
            };

            await _migrationLogRepository.SaveAsync(migrationLog);
        }

        private static string GetMigrationError(MigrationContext context, Exception exception) =>
            $"Error while executing migration number {context.Number} from assembly {context.MigrationSource}. Reason: {exception.Message}";
    }
}
