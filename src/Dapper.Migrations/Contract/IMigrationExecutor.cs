namespace WB.Dapper.Migrations.Contract
{
    public interface IMigrationExecutor
    {
        Task MigrateDatabaseAsync();
    }
}
