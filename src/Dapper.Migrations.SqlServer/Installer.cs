﻿using Microsoft.Extensions.DependencyInjection;
using WB.Dapper.Migrations.Shared;

namespace WB.Dapper.Migrations.SqlServer
{
    public static class Installer
    {
        public static void UseSqlServer(this DapperMigrationsOptions options, string connectionString)
        {
            options.RegisterConnectionString(connectionString);
            options.RegisterComponents(services =>
            {
                services.AddScoped<IMigrationExecutedRepository, SqlServerMigrationExecutedRepository>();
            });
        }
    }
}

