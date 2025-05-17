using WB.Dapper.Migrations.Shared;

namespace WB.Dapper.Migrations.Sqlite
{
    internal partial class SqliteMigrationExecutedRepository
    {
        private const string MigrationTableName = "MIG_Migrations";

        private const string CheckMigrationTableExistsQuery = $@"
SELECT count(*) FROM sqlite_master WHERE type='table' AND name='{MigrationTableName}'";

        private const string CreateMigrationTableQuery = @$"
CREATE TABLE {MigrationTableName} (
     {nameof(MigrationExecuted.Id)}             TEXT PRIMARY KEY
    ,{nameof(MigrationExecuted.Source)}         TEXT NOT NULL
    ,{nameof(MigrationExecuted.Number)}         INTEGER NOT NULL
    ,{nameof(MigrationExecuted.Name)}           TEXT NOT NULL
    ,{nameof(MigrationExecuted.Timestamp)}      TEXT NOT NULL
    ,{nameof(MigrationExecuted.Describtion)}    TEXT
)";

        private const string GetExecutedMigrationsQuery = $@"
SELECT 
    {nameof(MigrationExecuted.Id)}
    ,{nameof(MigrationExecuted.Source)}
    ,{nameof(MigrationExecuted.Number)}
    ,{nameof(MigrationExecuted.Name)}
    ,{nameof(MigrationExecuted.Timestamp)}
    ,{nameof(MigrationExecuted.Describtion)}
FROM {MigrationTableName}";

        private const string InsertExecutedMigrationQuery = $@"
INSERT INTO {MigrationTableName}
(
     {nameof(MigrationExecuted.Id)}
    ,{nameof(MigrationExecuted.Source)}
    ,{nameof(MigrationExecuted.Number)}
    ,{nameof(MigrationExecuted.Name)}
    ,{nameof(MigrationExecuted.Timestamp)}
    ,{nameof(MigrationExecuted.Describtion)}
) VALUES (
     @{nameof(MigrationExecuted.Id)}
    ,@{nameof(MigrationExecuted.Source)}
    ,@{nameof(MigrationExecuted.Number)}
    ,@{nameof(MigrationExecuted.Name)}
    ,@{nameof(MigrationExecuted.Timestamp)}
    ,@{nameof(MigrationExecuted.Describtion)}
)";
    }
}
