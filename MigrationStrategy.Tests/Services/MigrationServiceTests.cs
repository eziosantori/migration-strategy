using MigrationStrategy.Core.Interfaces;
using MigrationStrategy.Core.Models;
using MigrationStrategy.Core.Services;
using Moq;
using System;
using Xunit;

namespace MigrationStrategy.Tests.Services
{
    public class MigrationServiceTests
    {
        private readonly Mock<IPersistenceManager> _mockPersistenceManager;
        private readonly MigrationService _migrationService;

        public MigrationServiceTests()
        {
            // Common setup for all tests
            _mockPersistenceManager = new Mock<IPersistenceManager>();
            _migrationService = new MigrationService(_mockPersistenceManager.Object);
        }

        [Fact]
        public void Constructor_ShouldRequirePersistenceManager()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MigrationService(null));
        }

        [Fact]
        public void Constructor_WithValidPersistenceManager_ShouldCreateInstance()
        {
            // Assert
            Assert.NotNull(_migrationService);
        }

        [Fact]
        public void Migrate_ShouldRequireNonNullObject()
        {
            // Arrange
            IMoveItem migrationObject = null;
            var group = new Group("TestGroup");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                _migrationService.Migrate(migrationObject, group, MigrationType.COPY_SINGLE));
        }

        [Fact]
        public void Migrate_ShouldRequireNonNullGroup()
        {
            // Arrange
            var product = new Product("TestProduct");
            Group group = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                _migrationService.Migrate(product, group, MigrationType.COPY_SINGLE));
        }

        [Fact]
        public void Migrate_ShouldSupportProductAsObject()
        {
            // Arrange
            var product = new Product("TestProduct");
            var group = new Group("TestGroup");

            // Act
            // We're just testing that it doesn't throw an exception for now
            _migrationService.Migrate(product, group, MigrationType.COPY_SINGLE);

            // Assert - no exception means the test passes
        }

        [Fact]
        public void Migrate_ShouldSupportCategoryAsObject()
        {
            // Arrange
            var category = new Category("TestCategory");
            var group = new Group("TestGroup");

            // Act
            // We're just testing that it doesn't throw an exception for now
            _migrationService.Migrate(category, group, MigrationType.COPY_SINGLE);

            // Assert - no exception means the test passes
        }


    }
}