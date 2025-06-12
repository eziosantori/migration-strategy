using Microsoft.EntityFrameworkCore;
using MigrationStrategy.Core.Interfaces;

namespace MigrationStrategy.Core.Persistence.EntityFramework
{
    /// <summary>
    /// Factory for creating Entity Framework-based persistence managers.
    /// </summary>
    public static class EntityFrameworkPersistenceManagerFactory
    {
        /// <summary>
        /// Creates an in-memory Entity Framework persistence manager for testing purposes.
        /// </summary>
        /// <param name="databaseName">Name of the in-memory database to use.</param>
        /// <returns>An instance of IPersistenceManager that uses EF Core with an in-memory database.</returns>
        public static IPersistenceManager CreateInMemory(string databaseName = "MigrationStrategyTestDb")
        {
            var options = new DbContextOptionsBuilder<MigrationDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            var context = new MigrationDbContext(options);
            return new EntityFrameworkPersistenceManager(context);
        }

        /// <summary>
        /// Creates a SQL Server-based Entity Framework persistence manager for production use.
        /// </summary>
        /// <param name="connectionString">The connection string to the SQL Server database.</param>
        /// <returns>An instance of IPersistenceManager that uses EF Core with SQL Server.</returns>
        public static IPersistenceManager CreateSqlServer(string connectionString)
        {
            var options = new DbContextOptionsBuilder<MigrationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            var context = new MigrationDbContext(options);

            // Ensure database is created
            context.Database.EnsureCreated();

            return new EntityFrameworkPersistenceManager(context);
        }
    }
}
