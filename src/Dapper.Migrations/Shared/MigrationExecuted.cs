namespace WB.Dapper.Migrations.Shared
{
    public class MigrationExecuted
    {
        public Guid Id { get; init; }
        public required string Source { get; init; }
        public required int Number { get; init; }
        public required string Name { get; init; }
        public string? Describtion { get; init; }
        public required DateTimeOffset Timestamp { get; init; }
    }
}
