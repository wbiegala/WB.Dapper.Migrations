using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using WB.Dapper.Migrations.Contract;
using WB.Dapper.Migrations.Core;
using WB.Dapper.Migrations.Shared;

namespace WB.Dapper.Migrations
{
    public static class Installer
    {
        public static IServiceCollection AddDapperMigrations(
            this IServiceCollection services,
            Action<DapperMigrationsOptions> configure,
            params Assembly[] assemblies)
        {
            var options = new DapperMigrationsOptions(services);
            configure(options);

            services.AddScoped<IMigrationProvider, MigrationProvider>();
            services.AddScoped<IMigrationExecutor, MigrationExecutor>();

            if (!services.Any(s => s.ServiceType == typeof(IMigrationExecutedRepository)))
            {
                throw new InvalidOperationException($"No implementation of {nameof(IMigrationExecutedRepository)}. Check if you choose any database provider.");
            }

            services.TryAddScoped<ISqlConnectionProvider>(_ => new BasicSqlConnectionProvider(options.ConnectionString));

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
    }
}
