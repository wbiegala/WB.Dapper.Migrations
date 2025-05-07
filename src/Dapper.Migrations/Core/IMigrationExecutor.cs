namespace WB.Dapper.Migrations.Core
{
    internal interface IMigrationExecutor
    {
        Task MigrateDatabaseAsync();
    }
}
