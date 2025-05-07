namespace WB.Dapper.Migrations.Core.SqlServer
{
    internal partial class SqlServerMigrationExecutedRepository
    {
        private const string MigrationSchema = "Mig";
        private const string MigrationTableName = "Migrations";

        private const string CheckMigrationSchemaExistsQuery = $@"
SELECT COUNT(*) FROM sys.schemas WHERE name='{MigrationSchema}'";

        private const string CreateMigrationSchemaQuery = $@"
CREATE SCHEMA {MigrationSchema}";

        private const string CheckMigrationTableExistsQuery = @$"
SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='{MigrationSchema}' AND TABLE_NAME='{MigrationTableName}'";

        private const string CreateMigrationTableQuery = @$"
CREATE TABLE {MigrationSchema}.{MigrationTableName} (
     [{nameof(MigrationExecuted.Id)}]             UNIQUEIDENTIFIER PRIMARY KEY CLUSTERED DEFAULT NEWSEQUENTIALID()
    ,[{nameof(MigrationExecuted.Source)}]         NVARCHAR(512) NOT NULL
    ,[{nameof(MigrationExecuted.Number)}]         INT NOT NULL
    ,[{nameof(MigrationExecuted.Name)}]           NVARCHAR(128) NOT NULL
    ,[{nameof(MigrationExecuted.Timestamp)}]      DATETIMEOFFSET NOT NULL
    ,[{nameof(MigrationExecuted.Describtion)}]    NVARCHAR(256)
)";

        private const string GetExecutedMigrationsQuery = $@"
SELECT 
    [{nameof(MigrationExecuted.Id)}]
    ,[{nameof(MigrationExecuted.Source)}]
    ,[{nameof(MigrationExecuted.Number)}]
    ,[{nameof(MigrationExecuted.Name)}]
    ,[{nameof(MigrationExecuted.Timestamp)}]
    ,[{nameof(MigrationExecuted.Describtion)}]
FROM [{MigrationSchema}].[{MigrationTableName}]";

        private const string InsertExecutedMigrationQuery = $@"
INSERT INTO [{MigrationSchema}].[{MigrationTableName}]
(
    [{nameof(MigrationExecuted.Source)}]
    ,[{nameof(MigrationExecuted.Number)}]
    ,[{nameof(MigrationExecuted.Name)}]
    ,[{nameof(MigrationExecuted.Timestamp)}]
    ,[{nameof(MigrationExecuted.Describtion)}]
) VALUES (
    @{nameof(MigrationExecuted.Source)}
    ,@{nameof(MigrationExecuted.Number)}
    ,@{nameof(MigrationExecuted.Name)}
    ,@{nameof(MigrationExecuted.Timestamp)}
    ,@{nameof(MigrationExecuted.Describtion)}
)";
    }
}
