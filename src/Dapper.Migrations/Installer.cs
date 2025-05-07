using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WB.Dapper.Migrations.Contract;
using WB.Dapper.Migrations.Core;
using WB.Dapper.Migrations.Core.SqlServer;

namespace WB.Dapper.Migrations
{
    public static class Installer
    {
        public static IServiceCollection AddDapperMigrations(
            this IServiceCollection services,
            Action<DapperMigrationsOptions> configure,
            params Assembly[] assemblies)
        {
            var options = new DapperMigrationsOptions();
            configure(options);
            services.AddDatabaseProvider(options);

            services.AddScoped<IMigrationProvider, MigrationProvider>();
            services.AddScoped<IMigrationExecutor, MigrationExecutor>();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes()
                    .Where(t => t.IsClass == true && !t.IsAbstract && t.IsAssignableTo(typeof(IMigration)));

                foreach (var type in types)
                {
                    services.Add(new ServiceDescriptor(typeof(IMigration), type, ServiceLifetime.Scoped));
                }
            }

            return services;
        }

        private static IServiceCollection AddDatabaseProvider(this IServiceCollection services, DapperMigrationsOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.ConnectionString))
                throw new InvalidOperationException("Connection string is not set.");

            services.AddScoped<ISqlConnectionProvider>(_ => new SqlConnectionProvider(options.ConnectionString));

            switch (options.DatabaseProvider)
            {
                case DatabaseProvider.SqlServer:
                    services.AddScoped<IMigrationExecutedRepository, SqlServerMigrationExecutedRepository>();
                    break;
                default:
                    throw new NotSupportedException($"Database provider '{options.DatabaseProvider}' is not supported.");
            }

            return services;
        }
    }
}
