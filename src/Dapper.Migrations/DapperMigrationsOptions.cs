using Microsoft.Extensions.DependencyInjection;

namespace WB.Dapper.Migrations
{
    public sealed class DapperMigrationsOptions
    {
        private readonly IServiceCollection _services;
        
        internal string ConnectionString { get; private set; } = string.Empty;

        public DapperMigrationsOptions(IServiceCollection services)
        {
            _services = services;
        }

        public void RegisterConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void RegisterComponents(Action<IServiceCollection> configure)
        {
            configure(_services);
        }
    }
}
