using System.Reflection;
using WB.Dapper.Migrations.Contract;

namespace WB.Dapper.Migrations.Core
{
    internal class MigrationProvider : IMigrationProvider
    {
        private readonly IEnumerable<IMigration> _migrations;

        public MigrationProvider(IEnumerable<IMigration> migrations)
        {
            _migrations = migrations ?? throw new ArgumentNullException(nameof(migrations));
        }

        public IEnumerable<MigrationContext> GetMigrations()
        {
            var result = new List<MigrationContext>();

            foreach (var migration in _migrations)
            {
                var migrationType = migration.GetType();
                var migrationSource = migrationType.Assembly;
                var migrationAttribute = migrationType
                    .GetCustomAttribute<MigrationAttribute>();

                if (migrationAttribute is null)
                {
                    //TODO: log why skipped
                    continue;
                }

                var context = new MigrationContext(
                    migration: migration,
                    number: migrationAttribute.Number,
                    name: migrationAttribute.Name,
                    describtion: migrationAttribute.Describtion,
                    migrationSource: migrationSource.GetName().Name);

                result.Add(context);
            }

            return result;
        }
    }
}
