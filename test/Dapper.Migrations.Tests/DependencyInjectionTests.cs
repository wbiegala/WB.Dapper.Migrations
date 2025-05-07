using Microsoft.Extensions.DependencyInjection;
using WB.Dapper.Migrations.Contract;
using WB.Dapper.Migrations.Core;
using WB.Dapper.Migrations.Core.SqlServer;

namespace WB.Dapper.Migrations.Tests
{
    public class DependencyInjectionTests
    {
        [Fact]
        public void AddDapperMigrations_ValidConfigurationForSqlServer_RegistersServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var connectionString = "YourConnectionString";
            var assemblies = new[] { typeof(DependencyInjectionTests).Assembly };

            // Act
            services.AddDapperMigrations(options =>
            {
                options.UseSqlServer(connectionString);
            }, assemblies);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
  
            Assert.NotNull(serviceProvider.GetService<IMigrationProvider>());
            Assert.NotNull(serviceProvider.GetService<IMigrationExecutor>());
            Assert.NotNull(serviceProvider.GetService<ISqlConnectionProvider>());
            var migrationExecutedRepository = serviceProvider.GetService<IMigrationExecutedRepository>();
            Assert.True(migrationExecutedRepository is SqlServerMigrationExecutedRepository);
        }
    }
}
