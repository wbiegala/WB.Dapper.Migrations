namespace WB.Dapper.Migrations.Contract
{
    /// <summary>
    /// Interface for a migration.
    /// </summary>
    public interface IMigration
    {
        /// <summary>
        /// Executes migration. In this method, you should implement the logic of your migration.
        /// </summary>
        Task MigrateAsync();
    }
}
