namespace WB.Dapper.Migrations.Contract
{
    public interface IMigration
    {
        Task MigrateAsync();
    }
}
