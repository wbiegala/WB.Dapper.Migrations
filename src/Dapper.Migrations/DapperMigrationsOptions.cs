namespace WB.Dapper.Migrations
{
    public sealed class DapperMigrationsOptions
    {
        internal DatabaseProvider DatabaseProvider { get; private set; }
        internal string ConnectionString { get; private set; } = string.Empty;

        public void UseSqlServer(string connectionString)
        {
            DatabaseProvider = DatabaseProvider.SqlServer;
            ConnectionString = connectionString;
        }
    }
}
