using WB.Dapper.Migrations.Contract;

namespace WB.Dapper.Migrations.Core
{
    internal sealed class MigrationContext
    {
        private readonly IMigration _migration;

        public int Number { get; }
        public string Name { get; }
        public string? Describtion { get; }
        public string MigrationSource { get; }

        public async Task MigrateAsync()
        {
            await _migration.MigrateAsync();
        }

        public MigrationContext(IMigration migration, int number, string name, string? describtion, string migrationSource)
        {
            _migration = migration;
            Number = number;
            Name = name;
            Describtion = describtion;
            MigrationSource = migrationSource;
        }
    }
}
