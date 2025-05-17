using System.Text;
using Moq;
using WB.Dapper.Migrations.Contract;
using WB.Dapper.Migrations.Contract.Exceptions;
using WB.Dapper.Migrations.Core;
using WB.Dapper.Migrations.Shared;

namespace WB.Dapper.Migrations.Tests
{
    public class MigrationExecutorTests
    {
        [Fact]
        public async Task MigrateDatabaseAsync_WhenEmptyDatabase_InstallAllMigrations()
        {
            _migrationLogRepository.Setup(repo => repo.GetExecutedMigrationsAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Enumerable.Empty<MigrationExecuted>()));

            var executor = new MigrationExecutor(Provider, _migrationLogRepository.Object);
            await executor.MigrateDatabaseAsync();

            _migrationLogRepository.Verify(repo => repo.EnsureContextExistsAsync(It.IsAny<CancellationToken>()), Times.Once);
            _migrationLogRepository.Verify(repo => repo.GetExecutedMigrationsAsync(It.IsAny<CancellationToken>()), Times.Once);
            _migrationLogRepository.Verify(repo => repo.SaveAsync(It.IsAny<MigrationExecuted>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
        }

        [Fact]
        public async Task MigrateDatabaseAsync_WhenSomeMigrationsAlreadyInstalled_InstallMissingMigrations()
        {
            var installedMigrations = new List<MigrationExecuted>() { Migration1_Log, Migration2_Log };
            _migrationLogRepository.Setup(repo => repo.GetExecutedMigrationsAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(installedMigrations.AsEnumerable()));

            var executor = new MigrationExecutor(Provider, _migrationLogRepository.Object);
            await executor.MigrateDatabaseAsync();

            _migrationLogRepository.Verify(repo => repo.EnsureContextExistsAsync(It.IsAny<CancellationToken>()), Times.Once);
            _migrationLogRepository.Verify(repo => repo.GetExecutedMigrationsAsync(It.IsAny<CancellationToken>()), Times.Once);
            _migrationLogRepository.Verify(repo => repo.SaveAsync(It.IsAny<MigrationExecuted>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task MigrateDatabaseAsync_WhenAllMigrationsAlreadyInstalled_SkipInstallingMigrations()
        {
            var installedMigrations = new List<MigrationExecuted>() { Migration1_Log, Migration2_Log, Migration3_Log };
            _migrationLogRepository.Setup(repo => repo.GetExecutedMigrationsAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(installedMigrations.AsEnumerable()));

            var executor = new MigrationExecutor(Provider, _migrationLogRepository.Object);
            await executor.MigrateDatabaseAsync();

            _migrationLogRepository.Verify(repo => repo.EnsureContextExistsAsync(It.IsAny<CancellationToken>()), Times.Once);
            _migrationLogRepository.Verify(repo => repo.GetExecutedMigrationsAsync(It.IsAny<CancellationToken>()), Times.Once);
            _migrationLogRepository.Verify(repo => repo.SaveAsync(It.IsAny<MigrationExecuted>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task MigrateDatabaseAsync_WhenThereIsNoDatabaseConnection_SkipDatabaseMigrating()
        {
            _migrationLogRepository.Setup(repo => repo.EnsureContextExistsAsync(It.IsAny<CancellationToken>()))
                .Throws<TimeoutException>();

            var executor = new MigrationExecutor(Provider, _migrationLogRepository.Object);
            await Assert.ThrowsAsync<MigrationException>(executor.MigrateDatabaseAsync);

            _migrationLogRepository.Verify(repo => repo.SaveAsync(It.IsAny<MigrationExecuted>(), It.IsAny<CancellationToken>()), Times.Never);
            _migrationLogRepository.Verify(repo => repo.GetExecutedMigrationsAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task MigrateDatabaseAsync_WhenOneMigrationFailes_SkipInstallingRestOfMigrations()
        {
            var installedMigrations = new List<MigrationExecuted>() { Migration1_Log, Migration2_Log, Migration3_Log };
            _migrationLogRepository.Setup(repo => repo.GetExecutedMigrationsAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(installedMigrations.AsEnumerable()));

            var executor = new MigrationExecutor(FailedMigrationProvider, _migrationLogRepository.Object);
            await Assert.ThrowsAsync<MigrationException>(executor.MigrateDatabaseAsync);

            _migrationLogRepository.Verify(repo => repo.EnsureContextExistsAsync(It.IsAny<CancellationToken>()), Times.Once);
            _migrationLogRepository.Verify(repo => repo.GetExecutedMigrationsAsync(It.IsAny<CancellationToken>()), Times.Once);
            _migrationLogRepository.Verify(repo => repo.SaveAsync(It.IsAny<MigrationExecuted>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private readonly Mock<IMigrationExecutedRepository> _migrationLogRepository = new();
        private static IMigrationProvider Provider => new MigrationProvider(AllMigrations);
        private static IMigrationProvider FailedMigrationProvider =>
            new MigrationProvider(AllMigrations.Append(new TestMigration004()).Append(new TestMigration005()));
        private static IEnumerable<IMigration> AllMigrations = new List<IMigration>()
        {
            new TestMigration001(),
            new TestMigration002(),
            new TestMigration003()
        };

        private static MigrationExecuted Migration1_Log => new MigrationExecuted
        {
            Id = Guid.NewGuid(),
            Number = 1,
            Name = "test migration 1",
            Describtion = "initial migration",
            Source = typeof(TestMigration001).Assembly.GetName().Name!,
            Timestamp = new DateTimeOffset(2025, 1, 1, 12, 0, 0, TimeSpan.Zero),
        };

        private static MigrationExecuted Migration2_Log => new MigrationExecuted
        {
            Id = Guid.NewGuid(),
            Number = 2,
            Name = "test migration 2",
            Describtion = "doing something 1",
            Source = typeof(TestMigration001).Assembly.GetName().Name!,
            Timestamp = new DateTimeOffset(2025, 1, 1, 12, 0, 1, TimeSpan.Zero),
        };

        private static MigrationExecuted Migration3_Log => new MigrationExecuted
        {
            Id = Guid.NewGuid(),
            Number = 3,
            Name = "test migration 3",
            Describtion = "doing something 2",
            Source = typeof(TestMigration001).Assembly.GetName().Name!,
            Timestamp = new DateTimeOffset(2025, 1, 1, 12, 0, 2, TimeSpan.Zero),
        };

        [Migration(1, "test migration 1", "initial migration")]
        private class TestMigration001 : IMigration
        {
            public async Task MigrateAsync()
            {
                await Task.Delay(50);
            }
        }

        [Migration(2, "test migration 2", "doing something 1")]
        private class TestMigration002 : IMigration
        {
            public async Task MigrateAsync()
            {
                await Task.Delay(50);
            }
        }

        [Migration(3, "test migration 3", "doing something 2")]
        private class TestMigration003 : IMigration
        {
            public async Task MigrateAsync()
            {
                await Task.Delay(50);
            }
        }

        [Migration(4, "failed migration", "migration throws exception")]
        private class TestMigration004 : IMigration
        {
            public async Task MigrateAsync()
            {
                await Task.Delay(50);
                throw new InvalidOperationException();
            }
        }

        [Migration(5, "test migration 3", "doing something 3")]
        private class TestMigration005 : IMigration
        {
            public async Task MigrateAsync()
            {
                await Task.Delay(50);
            }
        }
    }
}
