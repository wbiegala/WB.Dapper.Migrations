namespace WB.Dapper.Migrations.Core
{
    internal interface IMigrationProvider
    {
        IEnumerable<MigrationContext> GetMigrations();
    }
}
