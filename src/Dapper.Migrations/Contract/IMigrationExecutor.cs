using WB.Dapper.Migrations.Contract.Exceptions;

namespace WB.Dapper.Migrations.Contract
{
    /// <summary>
    /// Interface for executing migrations.
    /// </summary>
    public interface IMigrationExecutor
    {
        /// <summary>
        /// Executes all migrations in the database.
        /// </summary>
        /// <exception cref="MigrationException"></exception>
        Task MigrateDatabaseAsync();
    }
}
