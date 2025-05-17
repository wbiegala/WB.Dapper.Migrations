# What is WB.Dapper.Migrations?
**WB.Dapper.Migrations** is tool to define and execute SQL database migrations in your application.
Tool supports SQL Server and SQLite databases.

## Use cases
1. Define migration logic strong connected to database provider.
2. Execute migrations in right order.

## Dependencies

### WB.Dapper.Migrations

- Dapper
- Microsoft.Data.SqlClient
- Microsoft.Extensions.DependencyInjection

### WB.Dapper.Migrations.Sqlite

- Microsoft.Data.Sqlite.Core
- SQLitePCLRaw.bundle_e_sqlite3
- SQLitePCLRaw.core

# Usage
## Installation
### - SQL Server
### - Sqlite

## Implementation
To implement your migration class you have to do three things:

1. Implement interface [*WB.Dapper.Migrations.Contract.IMigration*](./src/Dapper.Migrations/Contract/IMigration.cs) - write here your business logic of migration (example: create table, drop column etc.)
```csharp
internal sealed class Migration001_CreateTables : IMigration
{
    public Task MigrateAsync()
    {
        /*
         * YOUR CODE HERE
         */

        return Task.CompletedTask;
    }
}
```

2. Decorate implementation class by attribute [*WB.Dapper.Migrations.Contract.MigrationAttribute*](./src/Dapper.Migrations/Contract/MigrationAttribute.cs) - mark here execution order, name of migration and optional describtion
```csharp
[Migration(1, "Create all tables for model")]
internal sealed class Migration001_CreateTables : IMigration
{
    public Task MigrateAsync()
    {
        /*
         * YOUR CODE HERE
         */

        return Task.CompletedTask;
    }
}
```

3. Register your dependencies in `IServiceCollection`:
```csharp
services.AddDapperMigrations(options =>
{
    options.UseSqlite("your connection string");
}, typeof(Database).Assembly);
```

## Execution
To execute migrations, you have to call method `MigrateDatabaseAsync()` from service [*WB.Dapper.Migrations.Contract.IMigrationExecutor*](./src/Dapper.Migrations/Contract/IMigrationExecutor.cs)
```csharp
using (var appScope = appContainer.CreateScope())
{
    var migratior = appScope.ServiceProvider.GetRequiredService<IMigrationExecutor>();
    await migratior.MigrateDatabaseAsync();
}
```

# TL;DR
## How works `IMigrationExecutor`
1. It takes all registered migrations defined in given assemblies that are decorated by [*WB.Dapper.Migrations.Contract.MigrationAttribute*](./src/Dapper.Migrations/Contract/MigrationAttribute.cs), sort them by assembly name (named internally 'Source') and number,
and next for every migration checks if migration is executed already and if not - executes it.
2. Execution of migration is notified in database table
    - SQL Server: `[Mig].[Migrations]`
    - Sqlite: `MIG_Migrations`
3. [Model of executed migration](./src/Dapper.Migrations/Shared/MigrationExecuted.cs):
```csharp
public class MigrationExecuted
{
    public Guid Id { get; init; }
    public required string Source { get; init; }
    public required int Number { get; init; }
    public required string Name { get; init; }
    public string? Describtion { get; init; }
    public required DateTimeOffset Timestamp { get; init; }
}
```
4. When migration fail, executor will break iteration and throw [*WB.Dapper.Migrations.Contract.Exceptions.MigrationException*](./src/Dapper.Migrations/Contract/Exceptions/MigrationException.cs), property `InnerException` contains concrete exception with fail details.

# [Demo](./demo/WB.Dapper.Migrations.Demo/)