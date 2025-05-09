namespace WB.Dapper.Migrations.Contract.Exceptions
{
    public sealed class MigrationException : Exception
    {
        public MigrationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
